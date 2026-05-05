# API Endpoint Documentation

This document describes the currently available HTTP API endpoints.

## Base URLs

- http://localhost:5095
- https://localhost:7070

## Authentication and Authorization

1. Call `POST /auth/login` to set the auth cookie.
2. The browser/client sends the cookie on protected endpoints automatically.

Role model:
- `User`: read endpoints (`GET`)
- `Admin`: write endpoints (`POST`, `PUT`, `DELETE`)

## Auth

### Login

- Method: `POST`
- Path: `/auth/login`
- Auth: none (`AllowAnonymous`)
- Request body:

```json
{
  "username": "admin",
  "password": "admin123"
}
```

- Success response (`200 OK`):

```json
{
  "message": "Logged in, auth cookie set.",
  "role": "Admin"
}
```

Demo credentials:
- Admin: `admin` / `admin123`
- User: `user` / `user123`

### Register

- Method: `POST`
- Path: `/auth/register`
- Auth: none (`AllowAnonymous`)
- Request body:

```json
{
  "username": "newuser",
  "password": "secret123"
}
```

### Logout

- Method: `POST`
- Path: `/auth/logout`
- Auth: cookie (if logged in)

### Me

- Method: `GET`
- Path: `/auth/me`
- Auth: cookie

## Teams

### Get all teams

- Method: `GET`
- Path: `/teams`
- Auth: `User`

### Get team by id

- Method: `GET`
- Path: `/teams/{id}`
- Auth: `User`

### Get team MVP

- Method: `GET`
- Path: `/teams/MVP/{teamId}`
- Auth: `User`

### Get team matches

- Method: `GET`
- Path: `/teams/team-matches/{teamId}`
- Auth: `User`

### Get team players

- Method: `GET`
- Path: `/teams/teamplayers/{teamId}`
- Auth: `User`

### Create team

- Method: `POST`
- Path: `/teams`
- Auth: `Admin`
- Request body (`CreateTeamDto`):

```json
{
  "name": "FC Tuna"
}
```

### Update team

- Method: `PUT`
- Path: `/teams/{id}`
- Auth: `Admin`
- Request body (`UpdateTeamDto`):

```json
{
  "name": "FC Tuna",
  "points": 42
}
```

### Delete team

- Method: `DELETE`
- Path: `/teams/{id}`
- Auth: `Admin`

## Players

### Get all players

- Method: `GET`
- Path: `/players`
- Auth: `User`

### Get player by id

- Method: `GET`
- Path: `/players/{id}`
- Auth: `User`

### Create player

- Method: `POST`
- Path: `/players`
- Auth: `Admin`
- Request body (`CreatePlayerDto`):

```json
{
  "name": "John Doe",
  "marketValue": 1000000,
  "age": 24,
  "position": "Forward",
  "teamId": 1
}
```

### Update player

- Method: `PUT`
- Path: `/players/{id}`
- Auth: `Admin`
- Request body (`UpdatePlayerDto`):

```json
{
  "name": "John Doe",
  "marketValue": 1200000,
  "age": 25,
  "position": "Forward",
  "teamId": 1
}
```

### Delete player

- Method: `DELETE`
- Path: `/players/{id}`
- Auth: `Admin`

## Player Stats

### Get all player stats

- Method: `GET`
- Path: `/players/player-stats`
- Auth: `User`

### Get player stats by id

- Method: `GET`
- Path: `/players/player-stats/{id}`
- Auth: `User`

### Create player stats

- Method: `POST`
- Path: `/players/player-stats`
- Auth: `Admin`
- Request body (`CreatePlayerStatsDto`):

```json
{
  "goals": 2,
  "assists": 1,
  "shotsOnTarget": 4,
  "touches": 38,
  "passesCompleted": 22,
  "score": 7.5,
  "playerId": 1,
  "matchId": 1
}
```

### Update player stats

- Method: `PUT`
- Path: `/players/player-stats/{id}`
- Auth: `Admin`
- Request body (`UpdatePlayerStatsDto`):

```json
{
  "goals": 3,
  "assists": 1,
  "shotsOnTarget": 5,
  "touches": 40,
  "passesCompleted": 25,
  "score": 8.1
}
```

### Delete player stats

- Method: `DELETE`
- Path: `/players/player-stats/{id}`
- Auth: `Admin`

## Coaches

### Get all coaches

- Method: `GET`
- Path: `/coaches`
- Auth: `User`

### Get coach by id

- Method: `GET`
- Path: `/coaches/{id}`
- Auth: `User`

### Create coach

- Method: `POST`
- Path: `/coaches`
- Auth: `Admin`
- Request body (`CreateCoachDto`):

```json
{
  "name": "Jane Smith",
  "age": 45,
  "experienceYrs": 20,
  "teamId": 1
}
```

### Update coach

- Method: `PUT`
- Path: `/coaches/{id}`
- Auth: `Admin`
- Request body (`UpdateCoachDto`):

```json
{
  "name": "Jane Smith",
  "age": 46,
  "experienceYrs": 21,
  "teamId": 1
}
```

### Delete coach

- Method: `DELETE`
- Path: `/coaches/{id}`
- Auth: `Admin`

## Matches

### Get all matches

- Method: `GET`
- Path: `/matches`
- Auth: `User`

### Get match by id

- Method: `GET`
- Path: `/matches/{id}`
- Auth: `User`

### Create match

- Method: `POST`
- Path: `/matches`
- Auth: `Admin`
- Request body (`CreateMatchDto`):

```json
{
  "date": "2026-03-22T16:00:00Z",
  "location": "National Stadium",
  "homeTeamId": 1,
  "awayTeamId": 2,
  "homeTeamScore": 2,
  "awayTeamScore": 1
}
```

### Update match

- Method: `PUT`
- Path: `/matches/{id}`
- Auth: `Admin`
- Request body (`UpdateMatchDto`):

```json
{
  "date": "2026-03-29T16:00:00Z",
  "location": "National Stadium",
  "homeTeamId": 1,
  "awayTeamId": 2,
  "homeTeamScore": 3,
  "awayTeamScore": 2
}
```

### Delete match

- Method: `DELETE`
- Path: `/matches/{id}`
- Auth: `Admin`

## Goals

### Get all goals

- Method: `GET`
- Path: `/goals`
- Auth: `User`

### Get goal by id

- Method: `GET`
- Path: `/goals/{id}`
- Auth: `User`

### Get goals by match

- Method: `GET`
- Path: `/goals/match/{matchId}`
- Auth: `User`

### Get goals by team

- Method: `GET`
- Path: `/goals/team/{teamId}`
- Auth: `User`

### Create goal

- Method: `POST`
- Path: `/goals`
- Auth: `Admin`
- Request body (`CreateGoalDto`):

```json
{
  "playerId": 1,
  "matchId": 1,
  "teamId": 1
}
```

### Delete goal

- Method: `DELETE`
- Path: `/goals/{id}`
- Auth: `Admin`

## Notes

- Many endpoints return `404 Not Found` when the requested id does not exist.
- Validation failures return structured validation errors (`400`).
- Write endpoints return success messages as plain strings in the response body.
