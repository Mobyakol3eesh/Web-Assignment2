# API Endpoint Documentation

This document describes the currently available HTTP API endpoints.

## Base URLs

- http://localhost:5095
- https://localhost:7070

## Authentication and Authorization

1. Call `POST /auth/login` to get an `accessToken`.
2. Send the token on protected endpoints:

```http
Authorization: Bearer <accessToken>
```

Role model:
- `User`: read endpoints (`GET`)
- `Admin`: write endpoints (`POST`, `PUT`)

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
  "accessToken": "<jwt>",
  "role": "Admin"
}
```

Demo credentials:
- Admin: `admin` / `admin123`
- User: `user` / `user123`

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
  "appearances": 1,
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
  "appearances": 2
}
```

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

## Notes

- Many endpoints return `404 Not Found` when the requested id does not exist.
- Validation failures return structured validation errors (`400`).
- Write endpoints return success messages as plain strings in the response body.
