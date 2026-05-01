"""
simulate_league.py

Generates a simulated 20-team league with players, coaches, matches and detailed
match events/stats for a 4-week period. Outputs JSON files in `scripts/output/`:
 - teams.json
 - coaches.json
 - players.json
 - matches.json
 - goals.json
 - player_stats.json

Usage: python simulate_league.py

The script is standalone (no external dependencies). It produces data shaped to
match your project's DTOs/entities (ids, names, player stats fields, goal events).
You can use the JSON files to seed your API or inspect them locally.
"""

import random
import json
from datetime import datetime, timedelta
from pathlib import Path

OUTPUT_DIR = Path(__file__).parent / "output"
OUTPUT_DIR.mkdir(exist_ok=True)

random.seed(42)

POSITIONS = [
    "GK",
    "DEF",
    "DEF",
    "DEF",
    "MID",
    "MID",
    "MID",
    "FWD",
    "FWD",
]

TEAM_ADJS = [
    "Red", "Blue", "Green", "Golden", "Silver", "Black", "White", "Royal", "Iron", "Swift",
]
TEAM_NOUNS = [
    "Tigers", "Eagles", "Sharks", "Wolves", "Lions", "Rovers", "United", "City", "Rangers", "Athletic",
]
FIRST_NAMES = [
    "Alex", "Chris", "Sam", "Jordan", "Taylor", "Jamie", "Morgan", "Casey", "Drew", "Pat",
    "Lee", "Max", "Cameron", "Riley", "Quinn", "Avery", "Kai", "Noah", "Ethan", "Liam",
]
LAST_NAMES = [
    "Smith", "Johnson", "Brown", "Williams", "Jones", "Davis", "Miller", "Wilson", "Moore", "Taylor",
]
COACH_FIRST = ["Marco","Carlos","Diego","Roberto","Anna","Eva","Sofia","Luca","Mateo","Oliver"]

# Helpers

def make_team_name(i):
    adj = TEAM_ADJS[i % len(TEAM_ADJS)]
    noun = TEAM_NOUNS[i % len(TEAM_NOUNS)]
    return f"{adj} {noun}"


def rand_name():
    return f"{random.choice(FIRST_NAMES)} {random.choice(LAST_NAMES)}"


def rand_coach_name(i):
    return f"{COACH_FIRST[i % len(COACH_FIRST)]} {random.choice(LAST_NAMES)}"

# Generators

def generate_teams(n=20, players_per_team_range=(16,20)):
    teams = []
    coaches = []
    players = []
    player_id = 1
    coach_id = 1
    team_id = 1
    for i in range(n):
        tname = make_team_name(i)
        coach = {
            "Id": coach_id,
            "Name": rand_coach_name(i),
            "Age": random.randint(32, 68),
            "ExperienceYrs": random.randint(1, 35),
            "TeamId": team_id,
            "TeamName": tname,
        }
        coaches.append(coach)
        coach_id += 1
        team = {
            "Id": team_id,
            "Name": tname,
            "Points": 0,
            "CoachName": coach["Name"]
        }
        teams.append(team)

        # create players
        num_players = random.randint(*players_per_team_range)
        for p in range(num_players):
            pos = random.choice(POSITIONS)
            player = {
                "Id": player_id,
                "Name": rand_name(),
                "Age": random.randint(17, 34),
                "Position": pos,
                "MarketValue": random.randint(100000, 5000000),
                "TeamId": team_id,
                "TeamName": tname
            }
            players.append(player)
            player_id += 1

        team_id += 1
    return teams, coaches, players


def schedule_matches(teams, start_date: datetime, weeks=4):
    # For each week, shuffle and pair teams as first half vs second half
    matches = []
    match_id = 1
    team_ids = [t["Id"] for t in teams]
    for w in range(weeks):
        week_date = start_date + timedelta(weeks=w)
        shuffled = team_ids.copy()
        random.shuffle(shuffled)
        for i in range(0, len(shuffled), 2):
            home = shuffled[i]
            away = shuffled[i+1]
            match = {
                "Id": match_id,
                "Date": (week_date + timedelta(days=random.randint(0,6))).isoformat(),
                "Location": f"Stadium {home}",
                "HomeTeamId": home,
                "AwayTeamId": away,
                "HomeTeamScore": 0,
                "AwayTeamScore": 0
            }
            matches.append(match)
            match_id += 1
    return matches


def simulate_matches(matches, players_by_team):
    goals = []
    player_stats = []
    goal_id = 1
    ps_id = 1
    used_stats_player_ids = set()

    for m in matches:
        # basic team strengths
        home_strength = random.uniform(0.8, 1.2)
        away_strength = random.uniform(0.8, 1.2)

        home_goals = max(0, int(random.gauss(1.2 * home_strength, 1)))
        away_goals = max(0, int(random.gauss(1.0 * away_strength, 1)))

        # small randomness
        if random.random() < 0.05:
            # occasional high scoring
            home_goals += random.randint(1,3)
        if random.random() < 0.05:
            away_goals += random.randint(1,3)

        m["HomeTeamScore"] = home_goals
        m["AwayTeamScore"] = away_goals

        # allocate goals to players
        def pick_scorers(team_id, count):
            team_players = players_by_team.get(team_id, [])
            if not team_players:
                return []
            scorers = random.choices(team_players, k=count)
            return scorers

        home_scorers = pick_scorers(m["HomeTeamId"], home_goals)
        away_scorers = pick_scorers(m["AwayTeamId"], away_goals)

        # create player stats per player in match (only those who played/scored or random subset)
        participating = set()

        for scorer in home_scorers:
            goals.append({
                "Id": goal_id,
                "PlayerId": scorer["Id"],
                "MatchId": m["Id"],
                "TeamId": m["HomeTeamId"],
                "TimeScored": f"{random.randint(1,90)}'",
                "ScorerName": scorer["Name"]
            })
            participating.add(scorer["Id"])
            goal_id += 1

        for scorer in away_scorers:
            goals.append({
                "Id": goal_id,
                "PlayerId": scorer["Id"],
                "MatchId": m["Id"],
                "TeamId": m["AwayTeamId"],
                "TimeScored": f"{random.randint(1,90)}'",
                "ScorerName": scorer["Name"]
            })
            participating.add(scorer["Id"])
            goal_id += 1

        # create exactly one player_stats row per match
        home_team_players = players_by_team.get(m["HomeTeamId"], [])
        away_team_players = players_by_team.get(m["AwayTeamId"], [])
        candidate_players = [p for p in home_team_players + away_team_players if p["Id"] not in used_stats_player_ids]

        if not candidate_players:
            candidate_players = home_team_players + away_team_players

        featured_player = None
        if home_scorers:
            featured_player = next((p for p in home_scorers if p["Id"] not in used_stats_player_ids), None)
        if featured_player is None and away_scorers:
            featured_player = next((p for p in away_scorers if p["Id"] not in used_stats_player_ids), None)
        if featured_player is None and candidate_players:
            featured_player = random.choice(candidate_players)

        if featured_player is None:
            continue

        pid = featured_player["Id"]
        goals_scored = sum(1 for g in goals if g["PlayerId"] == pid and g["MatchId"] == m["Id"])
        assists = max(0, goals_scored - random.randint(0, goals_scored))
        shots = random.randint(goals_scored, max(1, goals_scored + 4))
        shots_on_target = min(shots, random.randint(0, shots))
        touches = random.randint(10, 120)
        passes = random.randint(5, 80)
        rating = round(4.0 + random.random() * 6.0, 1)  # 4.0 - 10.0

        player_stats.append({
            "Id": ps_id,
            "Goals": goals_scored,
            "Assists": assists,
            "ShotsOnTarget": shots_on_target,
            "Touches": touches,
            "PassesCompleted": passes,
            "Score": rating,
            "PlayerId": pid,
            "MatchId": m["Id"],
            "PlayerName": featured_player["Name"],
            "MatchDate": m["Date"],
            "MatchLocation": m["Location"]
        })
        used_stats_player_ids.add(pid)
        ps_id += 1

    return matches, goals, player_stats


def main():
    teams, coaches, players = generate_teams(20, (16,20))

    # index players by team for quick lookup
    players_by_team = {}
    for p in players:
        players_by_team.setdefault(p["TeamId"], []).append(p)

    start_date = datetime.utcnow()
    matches = schedule_matches(teams, start_date, weeks=4)

    matches, goals, player_stats = simulate_matches(matches, players_by_team)

    # write outputs
    (OUTPUT_DIR / "teams.json").write_text(json.dumps(teams, indent=2))
    (OUTPUT_DIR / "coaches.json").write_text(json.dumps(coaches, indent=2))
    (OUTPUT_DIR / "players.json").write_text(json.dumps(players, indent=2))
    (OUTPUT_DIR / "matches.json").write_text(json.dumps(matches, indent=2))
    (OUTPUT_DIR / "goals.json").write_text(json.dumps(goals, indent=2))
    (OUTPUT_DIR / "player_stats.json").write_text(json.dumps(player_stats, indent=2))

    print(f"Wrote {len(teams)} teams, {len(coaches)} coaches, {len(players)} players, {len(matches)} matches")
    print(f"Wrote {len(goals)} goals and {len(player_stats)} player_stats to {OUTPUT_DIR}")
    print("Sample API seeding hint: curl -X POST http(s)://localhost:7070/teams -H 'Content-Type: application/json' -d @scripts/output/teams.json")


if __name__ == '__main__':
    main()
