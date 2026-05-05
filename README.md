# Tuna Soccer League

A full-stack soccer league manager with an ASP.NET Core 9 API and a React + TypeScript + Vite frontend. The API supports teams, players, coaches, matches, and player stats with JWT auth and role-based authorization. The frontend consumes the API for league management and stats browsing.

## Prerequisites

- Node.js 18+ (or latest LTS)
- .NET SDK 9.0+
- MySQL Server

## Setup

### Backend (API)

1. Go to the backend folder:

```bash
cd backend
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Configure the database connection in `appsettings.json`.

4. Create a `.env` file in the backend folder:

```env
DB_PASSWORD=your_mysql_password
JWT_KEY=your_long_secret_key_here
JWT_ISSUER=TunaLeagueAPI
JWT_AUDIENCE=TunaLeagueClients
JWT_EXPIRY_MINUTES=120
```

5. Apply migrations:

```bash
dotnet ef database update
```

6. Run the API:

```bash
dotnet run
```

API base URLs:
- http://localhost:5095
- https://localhost:7070

Swagger UI:
- http://localhost:5095/swagger
- https://localhost:7070/swagger

### Frontend (Web App)

1. Go to the frontend folder:

```bash
cd frontend
```

2. Install dependencies:

```bash
npm install
```

3. Start the dev server:

```bash
npm run dev
```

Vite dev server (default):
- http://localhost:5173

## (Optional) Seed the database with Python scripts

The backend includes scripts to generate sample league data and seed the API.

1. Make sure the API is running (see Backend setup).

2. Go to the scripts folder:

```bash
cd backend/scripts
```

3. (First time only) Install the `requests` dependency:

```bash
python -m pip install requests
```

4. Generate JSON data files in `scripts/output/`:

```bash
python simulate_league.py
```

5. Seed the API using the generated JSON files:

```bash
python seed_api.py
```

Notes:
- `seed_api.py` logs in as `admin` / `admin123` and posts data to the API.
- Update `BASE_URL` at the top of `seed_api.py` if your API runs on a different port.

## Authentication

Login endpoint:
- `POST /auth/login`

The login response returns:
- `message`
- `role`

Auth uses an HTTP cookie set on login. Browsers/clients send the cookie automatically on protected endpoints.

Demo users (hardcoded in `AuthController`):
- Admin: `admin` / `admin123`
- User: `user` / `user123`

## API Routes Used

For full details, see [backend/docs/API_ENDPOINTS.md](backend/docs/API_ENDPOINTS.md).

### Auth

- `POST /auth/login`
- `POST /auth/register`
- `POST /auth/logout`
- `GET /auth/me`

### Teams

- `GET /teams`
- `GET /teams/{id}`
- `GET /teams/MVP/{teamId}`
- `GET /teams/team-matches/{teamId}`
- `GET /teams/teamplayers/{teamId}`
- `POST /teams`
- `PUT /teams/{id}`
- `DELETE /teams/{id}`

### Players

- `GET /players`
- `GET /players/{id}`
- `POST /players`
- `PUT /players/{id}`
- `DELETE /players/{id}`

### Player Stats

- `GET /players/player-stats`
- `GET /players/player-stats/{id}`
- `POST /players/player-stats`
- `PUT /players/player-stats/{id}`
- `DELETE /players/player-stats/{id}`

### Coaches

- `GET /coaches`
- `GET /coaches/{id}`
- `POST /coaches`
- `PUT /coaches/{id}`
- `DELETE /coaches/{id}`

### Matches

- `GET /matches`
- `GET /matches/{id}`
- `POST /matches`
- `PUT /matches/{id}`
- `DELETE /matches/{id}`

### Goals

- `GET /goals`
- `GET /goals/{id}`
- `GET /goals/match/{matchId}`
- `GET /goals/team/{teamId}`
- `POST /goals`
- `DELETE /goals/{id}`
