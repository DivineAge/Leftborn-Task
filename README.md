# Leftborn-Task

## Run with Docker

Start the application using Docker Compose (builds images and runs containers):

```bash
docker compose up --build -d
```

View live logs:

```bash
docker compose logs -f
```

Stop and remove containers:

```bash
docker compose down
```

## Migrations (cross-platform)

Migrations are not applied automatically. To run migrations for all modules, choose one of the cross-platform options below.

- Using the .NET SDK (requires SDK installed): from the repository root run the EF Core `database update` command for each module:

```bash
dotnet ef database update --project src/Modules/Users/Module.Users.Infrastructure/Module.Users.Infrastructure.csproj --startup-project src/API/API.csproj

dotnet ef database update --project src/Modules/Songs/Module.Songs.Infrastructure/Module.Songs.Infrastructure.csproj --startup-project src/API/API.csproj

dotnet ef database update --project src/Modules/Playlists/Module.Playlist.Infrastructure/Module.Playlist.Infrastructure.csproj --startup-project src/API/API.csproj
```

- Using Docker (no host SDK required): start the API container, then execute the EF command inside the running API service. Replace `<api-service-name>` with your API service name from `docker compose ps`:

```bash
docker compose up --build -d
docker compose exec <api-service-name> dotnet ef database update --project src/Modules/Users/Module.Users.Infrastructure/Module.Users.Infrastructure.csproj --startup-project src/API/API.csproj

docker compose exec <api-service-name> dotnet ef database update --project src/Modules/Songs/Module.Songs.Infrastructure/Module.Songs.Infrastructure.csproj --startup-project src/API/API.csproj

docker compose exec <api-service-name> dotnet ef database update --project src/Modules/Playlists/Module.Playlist.Infrastructure/Module.Playlist.Infrastructure.csproj --startup-project src/API/API.csproj
```

Run these commands from the repository root. The Docker approach is cross-platform and does not require a host .NET SDK.