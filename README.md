# Luftborn Task

## Run with Docker Compose

The easiest way to run Luftborn Task locally is with Docker Compose.

### Prerequisites

- Docker and Docker Compose.
- .NET SDK.

### 1. Clone the repository

```bash
git clone <repository-url>
cd <repository-directory>
```

### 2. Start the application

Build the images and start the services in the background:

```bash
docker compose up --build -d
```

### 3. Install the Entity Framework Core CLI

Install the `dotnet-ef` tool before applying the database migrations:

```bash
dotnet tool install --global dotnet-ef
```

### 4. Create the database schemas and tables

Run the following commands from the **root directory of the project** after starting Docker Compose. The database services must be running before applying the Entity Framework Core migrations.

```bash
dotnet ef migrations add migrations \
  --project src/Modules/Songs/Module.Songs.Infrastructure/Module.Songs.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context SongsDbContext \
  --output-dir Database/Migrations
```

```bash
dotnet ef migrations update \
  --project src/Modules/Playlists/Module.Playlist.Infrastructure/Module.Playlist.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context PlaylistDbContext \
  --output-dir Database/Migrations
```

```bash
dotnet ef migrations add migrations \
  --project src/Modules/Users/Module.Users.Infrastructure/Module.Users.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context UsersDbContext \
  --output-dir Database/Migrations
```

### 5. Recreate migrations if necessary

If the migrations are missing or corrupted, you can delete the existing migration files and recreate them from the **root directory of the project** using the following commands:

```bash
dotnet ef migrations add migrations \
  --project src/Modules/Songs/Module.Songs.Infrastructure/Module.Songs.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context UsersDbContext \
  --output-dir Database/Migrations
```

```bash
dotnet ef migrations add migrations \
  --project src/Modules/Playlists/Module.Playlist.Infrastructure/Module.Playlist.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context PlaylistDbContext \
  --output-dir Database/Migrations
```

```bash
dotnet ef migrations add migrations \
  --project src/Modules/Users/Module.Users.Infrastructure/Module.Users.Infrastructure.csproj \
  --startup-project src/API/API.csproj \
  --context UsersDbContext \
  --output-dir Database/Migrations
```

> Only delete migration files if they are genuinely missing or corrupted. Recreating migrations may require a database reset or additional cleanup if the existing database already contains migration history.

### 6. Access the application

The backend API is available at:

```text
http://localhost:5000
```

Verify the running services and port mappings with:

```bash
docker compose ps
```

## Common Docker commands

View live logs:

```bash
docker compose logs -f
```

Show service status:

```bash
docker compose ps
```

Stop and remove the containers:

```bash
docker compose down
```

## Architecture design

Luftborn Task is a **modular monolith** built using **Clean Architecture**. It runs as one application, while each business capability is separated into an independent module.

### Modular monolith and Clean Architecture

Each module owns a specific business capability and contains its own domain, application, and infrastructure layers. Dependencies point inward toward the business logic, keeping it independent from frameworks and persistence details.

This provides clear module boundaries and many benefits of microservices without the deployment and operational complexity of multiple applications.

### Synchronous module communication

Modules communicate synchronously through internal public interfaces. These interfaces are not HTTP endpoints; their only purpose is to duplicate data that another module requires.

The receiving module stores and uses its own local copy instead of accessing another module's database. This keeps modules self-contained and preserves independent data ownership.

### Duplicated module data

Each module maintains its own database structure and required data. This prevents shared-table coupling and allows modules to change their schemas and persistence logic independently.

The main trade-off is data consistency: when data changes, the owning module must use the appropriate interface to update the copies maintained by other modules.

## Database structure

The database uses PostgreSQL and a normalized relational design with four main entities: users, songs, playlists, and playlist-song relationships.

### Why PostgreSQL?

PostgreSQL is a strong fit for Luftborn Task because it provides reliable relational data management, powerful constraints, and excellent support for the relationships used by the application.

- **Referential integrity:** Primary keys, foreign keys, and constraints protect relationships between users, songs, playlists, and playlist entries.
- **Transaction support:** ACID-compliant transactions help ensure that related changes are completed consistently.
- **Strong querying capabilities:** PostgreSQL efficiently supports joins, filtering, sorting, aggregation, and many-to-many queries.
- **Good .NET integration:** It works well with Entity Framework Core and supports code-first migrations for each module.
- **Reliability and scalability:** PostgreSQL is mature, open source, and suitable for both local development and production workloads.
- **Independent module databases:** Each module can have its own PostgreSQL database while retaining the same reliable relational features.

### Users

| Column | Type | Description |
| --- | --- | --- |
| `Id` | `Guid` | Primary key that uniquely identifies the user. |
| `FirstName` | `string` | User's first name. |
| `LastName` | `string` | User's last name. |
| `Email` | `string` | User's email address. |

### Songs

| Column | Type | Description |
| --- | --- | --- |
| `Id` | `Guid` | Primary key that uniquely identifies the song. |
| `PublisherId` | `Guid` | Foreign key referencing `Users.Id`. A user can publish many songs. |
| `Name` | `string` | Song name. |
| `TimeInSeconds` | `string` | Song duration in seconds. |

The relationship between users and songs is one-to-many: one user can publish multiple songs, while each song belongs to one publisher.

### Playlists

| Column | Type | Description |
| --- | --- | --- |
| `Id` | `Guid` | Primary key that uniquely identifies the playlist. |
| `OwnerId` | `Guid` | Foreign key referencing `Users.Id`. A user can own multiple playlists. |
| `Name` | `string` | Playlist name. |

The relationship between users and playlists is one-to-many: one user can own multiple playlists, while each playlist has one owner.

### PlaylistSongs

| Column | Type | Description |
| --- | --- | --- |
| `PlaylistId` | `Guid` | Foreign key referencing `Playlists.Id`. |
| `SongId` | `Guid` | Foreign key referencing `Songs.Id`. |

`PlaylistSongs` is a junction table representing the many-to-many relationship between playlists and songs. Its composite primary key prevents the same song from being added to the same playlist more than once.

## Why this structure is good

- **Clear relationships:** Foreign keys enforce ownership and publishing relationships.
- **Normalized data:** Separate tables reduce duplication within each module.
- **Many-to-many support:** `PlaylistSongs` efficiently connects playlists and songs.
- **Duplicate prevention:** Its composite key prevents duplicate playlist-song associations.
- **Module independence:** Local data copies prevent direct database coupling between modules.
- **Scalable identifiers:** `Guid` keys provide globally unique identifiers.

For production use, `TimeInSeconds` would generally be better stored as an integer such as `int` rather than `string`. This makes validation, sorting, filtering, and calculations more reliable.