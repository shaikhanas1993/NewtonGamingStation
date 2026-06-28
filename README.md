```bash
cd NewtonGamingStation
docker compose up --build
```

Then open:

- **Web app** → http://localhost:8080
- **API** → http://localhost:5080/health (OpenAPI doc at http://localhost:5080/openapi/v1.json)
- **Database** → `localhost,1433`, user `sa`, password `Your_strong_Pass123`

```bash
docker compose down -v
```

## Running locally without Docker

### Backend

```bash
# 1. Start just the database
docker compose up -d db

# 2. Run the API (applies migrations + seeds on startup)
cd backend
dotnet restore
dotnet run --project src/NewtonGamingStation.Api
# → http://localhost:5080
```

### Frontend (needs Node 20+/22)

```bash
cd frontend
npm install            # a committed .npmrc sets legacy-peer-deps=true (see note below)
npm start
# → http://localhost:4200  (proxies to the API at http://localhost:5080/api)
```

## Tests

### Backend — unit + integration

```bash
cd backend
dotnet test
```

- `NewtonGamingStation.UnitTests` — pure unit tests of the service layer with a mocked repository.
- `NewtonGamingStation.IntegrationTests` — boots the full HTTP pipeline via `WebApplicationFactory` ```

---

```bash
cd backend
# Add a migration
dotnet ef migrations add <Name> \
  --project src/NewtonGamingStation.Infrastructure \
  --startup-project src/NewtonGamingStation.Api

# Apply manually (the API also does this automatically on startup)
dotnet ef database update \
  --project src/NewtonGamingStation.Infrastructure \
  --startup-project src/NewtonGamingStation.Api
```

(Install the tool once with `dotnet tool install --global dotnet-ef` if needed.)

---

---

## Data model

| Table        | Notes                                                     |
| ------------ | --------------------------------------------------------- |
| `Publishers` | One publisher → many games                                |
| `Games`      | FK → Publishers; genre stored as int; indexed on Title    |
| `Roles`      | Reference data (Administrator / Editor / Viewer)          |
| `Users`      | Unique UserName + Email                                   |
| `UserRoles`  | Composite-key join (many-to-many between Users and Roles) |

## API endpoints

| Method | Route             | Purpose                                 |
| ------ | ----------------- | --------------------------------------- |
| GET    | `/api/games`      | Search / filter / sort / paginate       |
| GET    | `/api/games/{id}` | Get one                                 |
| POST   | `/api/games`      | Create                                  |
| PUT    | `/api/games/{id}` | Update                                  |
| DELETE | `/api/games/{id}` | Delete                                  |
| GET    | `/api/publishers` | List publishers (for the form dropdown) |
| GET    | `/health`         | Liveness probe                          |

`GET /api/games` query parameters: `search`, `genre`, `platform`, `publisherId`, `sortBy` (`title`|`price`|`releaseDate`), `desc`, `page`, `pageSize`.
