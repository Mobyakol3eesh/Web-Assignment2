# Tuna SoccerLeague API

ASP.NET Core 9 project for managing teams, players, coaches, matches, and player statistics.

The solution uses:
- Layered architecture (Controllers -> Services -> Data)
- DTO-based request/response models
- Entity Framework Core with MySQL
- JWT authentication and role-based authorization
- Swagger/OpenAPI for API testing

## Prerequisites

- .NET SDK 9.0 or later
- MySQL Server
- EF Core CLI tools (if not installed globally)

```bash
dotnet tool install --global dotnet-ef
```

## Getting Started

1. Clone the repository and move into the project folder.

```bash
git clone https://github.com/Mobyakol3eesh/Tuna-SoccerLeague.git
cd Tuna-SoccerLeague
```

2. Restore dependencies.

```bash
dotnet restore
```

3. Configure database connection.

Edit `appsettings.json` and set `ConnectionStrings:DefaultConnection` with your MySQL host, port, database, and username.

Password handling:
- The app reads `DB_PASSWORD` from `.env` (or environment variables) and injects it into the connection string at runtime.

4. Create a `.env` file in the project root.

```env
DB_PASSWORD=your_mysql_password
JWT_KEY=your_long_secret_key_here
JWT_ISSUER=TunaLeagueAPI
JWT_AUDIENCE=TunaLeagueClients
JWT_EXPIRY_MINUTES=120
```

5. Apply migrations.

```bash
dotnet ef database update
```

6. Run the application.

```bash
dotnet run
```

7. Open Swagger UI.

- http://localhost:5095/swagger
- https://localhost:7070/swagger

## API Endpoint Documentation

For a full endpoint reference (routes, methods, auth requirements, and request body examples), see:

- [docs/API_ENDPOINTS.md](docs/API_ENDPOINTS.md)

## Authentication

Login endpoint:

- `POST /auth/login`

The login response returns:
- `accessToken`
- `role`

Use the token in requests:

```http
Authorization: Bearer <accessToken>
```

Demo users currently hardcoded in `AuthController`:
- Admin: `admin` / `admin123`
- User: `user` / `user123`

## Authorization Rules

- `User` role: read endpoints (typically `GET`)
- `Admin` role: write endpoints (typically `POST`, `PUT`, `DELETE`)

## Project Structure

- `controllers/`: API and MVC controllers
- `services/`: business logic
- `interfaces/`: service contracts
- `models/`: EF Core entity models
- `dtos/`: create/update/read DTOs
- `database/`: EF Core DbContext
- `Migrations/`: EF Core migrations
- `utilities/`: startup and configuration helpers
- `views/`: MVC Razor views

## Tech Stack

- ASP.NET Core 9 (MVC + Web API)
- C#
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MySQL
- JWT Bearer Authentication
- Swagger (Swashbuckle)
- DotNetEnv

## Why HTTP-only Cookies Are Commonly Used

HTTP-only cookies are widely adopted in browser-based authentication because they reduce the chance of token theft through client-side scripts.

Key reasons they are treated as an industry standard:
- JavaScript cannot read them: when a cookie is marked `HttpOnly`, it is not accessible through `document.cookie`, which lowers XSS token-exfiltration risk.
- Strong browser controls: cookies support `Secure` (HTTPS only) and `SameSite` (`Lax` or `Strict`) attributes that help reduce interception and CSRF exposure.
- Centralized session handling: browsers automatically attach cookies to matching domains/paths, reducing custom token storage logic in frontend code.
- Better fit for web sessions: many production systems combine short-lived access cookies with refresh-token rotation and server-side revocation controls.

Important caveat:
- HTTP-only cookies improve security posture, but they are not a complete solution by themselves.
- Production systems still need HTTPS, CSRF protection, key rotation, short token lifetimes, and secure logout/invalidation strategy.

## Notes

- Swagger is enabled by default.
- This project currently uses bearer tokens in headers, not HTTP-only cookies.
- For production use, replace demo credentials, rotate keys, enforce HTTPS, and add stronger identity management.
