"""
seed_api.py

Logs in to the local Tuna League API using admin credentials (cookie-based auth),
then seeds teams, players, matches, goals and player_stats from the JSON files
produced by `simulate_league.py` (scripts/output/*.json).

Usage: python seed_api.py

Configuration options at top of file.
"""

import json
import time
from pathlib import Path
import requests
import urllib3

# Configuration
BASE_URL = "http://localhost:5095"
ADMIN_USERNAME = "admin"
ADMIN_PASSWORD = "admin123"
INPUT_DIR = Path(__file__).parent / "output"
VERIFY_SSL = False  # set to True if you have a trusted cert
SLEEP_BETWEEN = 0.05

if not INPUT_DIR.exists():
    raise SystemExit(f"Input directory not found: {INPUT_DIR}")

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

session = requests.Session()
session.verify = VERIFY_SSL

headers = {"Content-Type": "application/json"}


def get_value(item, *keys, default=None):
    for key in keys:
        if key in item:
            return item[key]
    return default


def login(username, password):
    url = f"{BASE_URL}/auth/login"
    payload = {"username": username, "password": password}
    print(f"Logging in as {username} -> {url}")
    r = session.post(url, json=payload, headers=headers)
    if r.status_code == 200:
        print("Login OK; cookies set:", session.cookies.get_dict())
        return True
    else:
        print("Login failed:", r.status_code, r.text)
        return False


def post_team(team_name):
    url = f"{BASE_URL}/teams"
    payload = {"name": team_name}
    r = session.post(url, json=payload, headers=headers)
    return r


def post_player(player):
    url = f"{BASE_URL}/players"
    payload = {
        "name": player.get("Name") or player.get("name"),
        "marketValue": player.get("MarketValue", 0),
        "age": player.get("Age", 18),
        "position": player.get("Position", "MID"),
        "teamId": player.get("TeamId")
    }
    r = session.post(url, json=payload, headers=headers)
    return r


def post_match(match):
    url = f"{BASE_URL}/matches"
    payload = {
        "date": match.get("Date"),
        "location": match.get("Location"),
        "homeTeamId": match.get("HomeTeamId"),
        "awayTeamId": match.get("AwayTeamId"),
        "homeTeamScore": match.get("HomeTeamScore", 0),
        "awayTeamScore": match.get("AwayTeamScore", 0)
    }
    r = session.post(url, json=payload, headers=headers)
    return r


def post_goal(goal):
    url = f"{BASE_URL}/goals"
    payload = {
        "playerId": goal.get("PlayerId"),
        "matchId": goal.get("MatchId"),
        "teamId": goal.get("TeamId")
        # omit TimeScored to let server default
    }
    r = session.post(url, json=payload, headers=headers)
    return r


def post_player_stats(ps):
    url = f"{BASE_URL}/players/player-stats"
    payload = {
        "goals": ps.get("Goals", 0),
        "assists": ps.get("Assists", 0),
        "shotsOnTarget": ps.get("ShotsOnTarget", 0),
        "touches": ps.get("Touches", 0),
        "passesCompleted": ps.get("PassesCompleted", 0),
        "score": ps.get("Score", 0),
        "playerId": ps.get("PlayerId"),
        "matchId": ps.get("MatchId")
    }
    r = session.post(url, json=payload, headers=headers)
    return r


def post_coach(coach):
    url = f"{BASE_URL}/coaches"
    payload = {
        "name": coach.get("Name") or coach.get("name"),
        "age": coach.get("Age", 40),
        "experienceYrs": coach.get("ExperienceYrs", 0),
        "teamId": coach.get("TeamId")
    }
    r = session.post(url, json=payload, headers=headers)
    return r


def get_server_teams():
    url = f"{BASE_URL}/teams"
    r = session.get(url, headers=headers)
    if r.status_code == 200:
        return r.json()
    raise Exception(f"Failed to fetch server teams: {r.status_code} {r.text}")


def get_server_players():
    url = f"{BASE_URL}/players"
    r = session.get(url, headers=headers)
    if r.status_code == 200:
        return r.json()
    raise Exception(f"Failed to fetch server players: {r.status_code} {r.text}")


def get_server_matches():
    url = f"{BASE_URL}/matches"
    r = session.get(url, headers=headers)
    if r.status_code == 200:
        return r.json()
    raise Exception(f"Failed to fetch server matches: {r.status_code} {r.text}")


def main():
    if not login(ADMIN_USERNAME, ADMIN_PASSWORD):
        raise SystemExit("Login failed; aborting.")

    # Load local JSONs
    teams = json.load(open(INPUT_DIR / "teams.json"))
    coaches = json.load(open(INPUT_DIR / "coaches.json")) if (INPUT_DIR / "coaches.json").exists() else []
    players = json.load(open(INPUT_DIR / "players.json"))
    matches = json.load(open(INPUT_DIR / "matches.json"))
    goals = json.load(open(INPUT_DIR / "goals.json"))
    player_stats = json.load(open(INPUT_DIR / "player_stats.json"))

    # 1) Post teams (by name)
    print("Posting teams...")
    for t in teams:
        r = post_team(t["Name"])
        print("POST /teams", t["Name"], r.status_code)
        time.sleep(SLEEP_BETWEEN)

    # 2) Refresh server teams and map names -> server id
    server_teams = get_server_teams()
    name_to_id = {get_value(t, "Name", "name"): get_value(t, "Id", "id") for t in server_teams}
    print("Mapped teams:", len(name_to_id))

    # Build original id -> name map from local teams.json
    orig_id_to_name = {t["Id"]: t["Name"] for t in teams}

    # 2b) Post coaches after teams so TeamId can be mapped safely
    if coaches:
        print("Posting coaches...")
        for coach in coaches:
            team_name = orig_id_to_name.get(coach.get("TeamId"))
            server_team_id = name_to_id.get(team_name)
            if not server_team_id:
                print("Skipping coach due to missing team mapping", coach)
                continue
            coach_payload = dict(coach)
            coach_payload["TeamId"] = server_team_id
            r = post_coach(coach_payload)
            print("POST /coaches", coach.get("Name") or coach.get("name"), r.status_code)
            time.sleep(SLEEP_BETWEEN)

    # 3) Post players using name->team mapping
    print("Posting players...")
    for p in players:
        # map original TeamId to server TeamId by name
        orig_team_id = p.get("TeamId")
        team_name = orig_id_to_name.get(orig_team_id)
        server_team_id = name_to_id.get(team_name)
        p_payload = dict(p)
        p_payload["TeamId"] = server_team_id
        r = post_player(p_payload)
        print("POST /players", p["Name"], "->", r.status_code)
        time.sleep(SLEEP_BETWEEN)

    # 4) Refresh server players and map (Name, TeamName) -> server player id
    server_players = get_server_players()
    player_key_to_id = {
        (get_value(pl, "Name", "name"), get_value(pl, "TeamName", "teamName", default="")): get_value(pl, "Id", "id")
        for pl in server_players
    }
    print("Mapped players:", len(player_key_to_id))

    # 5) Post matches - translate original team ids to server team ids
    print("Posting matches...")
    for m in matches:
        home_name = orig_id_to_name.get(m["HomeTeamId"])
        away_name = orig_id_to_name.get(m["AwayTeamId"])
        m_payload = dict(m)
        m_payload["HomeTeamId"] = name_to_id.get(home_name)
        m_payload["AwayTeamId"] = name_to_id.get(away_name)
        # keep Date ISO string as-is
        r = post_match(m_payload)
        print("POST /matches", m["Id"], "->", r.status_code)
        time.sleep(SLEEP_BETWEEN)

    # 6) Refresh server matches and build match mapping by Date+Home+Away
    server_matches = get_server_matches()
    # normalize date strings for matching (date only)
    def match_key(m):
        return (get_value(m, "Date", "date"), get_value(m, "HomeTeamName", "homeTeamName", default=""), get_value(m, "AwayTeamName", "awayTeamName", default=""))

    server_match_map = {match_key(m): get_value(m, "Id", "id") for m in server_matches}

    orig_matchid_to_key = {}
    for m in matches:
        home_name = orig_id_to_name.get(m["HomeTeamId"])
        away_name = orig_id_to_name.get(m["AwayTeamId"])
        orig_matchid_to_key[m["Id"]] = (m.get("Date"), home_name, away_name)

    # 7) Post goals - translate player/match/team ids to server ids
    print("Posting goals...")
    for g in goals:
        # find original player -> name/team
        orig_player = next((p for p in players if p["Id"] == g["PlayerId"]), None)
        if orig_player is None:
            print("Original player id not found", g["PlayerId"]); continue
        player_key = (orig_player["Name"], orig_id_to_name.get(orig_player["TeamId"]))
        server_player_id = player_key_to_id.get(player_key)
        if server_player_id is None:
            # try by name only
            candidates = [pl for pl in server_players if pl["Name"] == orig_player["Name"]]
            server_player_id = candidates[0]["Id"] if candidates else None
        orig_match_key = orig_matchid_to_key.get(g["MatchId"])
        if orig_match_key is None:
            print("Skipping goal due to missing match mapping", g)
            continue

        server_match_id = server_match_map.get(orig_match_key)
        server_team_id = name_to_id.get(orig_id_to_name.get(g["TeamId"]))
        if not server_player_id or not server_match_id or not server_team_id:
            print("Skipping goal due to missing mapping", g); continue
        payload = {"PlayerId": server_player_id, "MatchId": server_match_id, "TeamId": server_team_id}
        r = post_goal(payload)
        print("POST /goals", payload, r.status_code)
        time.sleep(SLEEP_BETWEEN)

    # 8) Post player_stats - translate ids and post
    print("Posting player_stats...")
    for ps in player_stats:
        orig_player = next((p for p in players if p["Id"] == ps["PlayerId"]), None)
        if orig_player is None:
            print("Original player not found", ps["PlayerId"]); continue
        player_key = (orig_player["Name"], orig_id_to_name.get(orig_player["TeamId"]))
        server_player_id = player_key_to_id.get(player_key)
        orig_match_key = orig_matchid_to_key.get(ps["MatchId"])
        if orig_match_key is None:
            print("Skipping player_stats due to missing match mapping", ps)
            continue

        server_match_id = server_match_map.get(orig_match_key)
        if not server_player_id or not server_match_id:
            print("Skipping player_stats due to missing mapping", ps); continue
        payload = dict(ps)
        payload["PlayerId"] = server_player_id
        payload["MatchId"] = server_match_id
        # keep other numeric fields as-is
        r = post_player_stats(payload)
        print("POST /players/player-stats", payload.get("PlayerId"), payload.get("MatchId"), r.status_code)
        time.sleep(SLEEP_BETWEEN)

    print("Seeding complete.")


if __name__ == '__main__':
    main()
