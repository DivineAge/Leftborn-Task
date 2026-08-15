# Luftborn Task

## Run with Docker Compose

The easiest way to run Luftborn Task locally is with Docker Compose.

### Prerequisites

- Docker and Docker Compose.

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

### 3. Access the application

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

Luftborn Task is designed as a **modular monolith** that follows **Clean Architecture** principles. The application is deployed as a single system, but its business capabilities are separated into independent modules with clear boundaries and responsibilities.

### Modular monolith

Each module owns a specific business capability and is organized internally into its own application, domain, and infrastructure concerns. Modules can be developed and reasoned about independently while still running within the same deployable application.

This approach provides many of the organizational benefits of microservices without introducing the operational complexity of multiple independently deployed services. It also keeps the application straightforward to run while preserving clear module boundaries.

### Clean Architecture

Each module follows a layered structure based on Clean Architecture:

- **Domain:** Contains entities, value objects, business rules, and domain behavior. This layer does not depend on external frameworks or infrastructure.
- **Application:** Contains use cases, commands, queries, interfaces, and application-specific business orchestration.
- **Infrastructure:** Contains database access, external integrations, persistence implementations, and framework-specific details.
- **API:** Exposes the module's functionality through the application and translates external requests into application use cases.

Dependencies point inward toward the domain and application layers. This keeps the core business logic isolated from databases, frameworks, and external services, making the modules easier to test, maintain, and evolve.

### Synchronous module communication

Modules communicate synchronously through a shared public interface. This public API is an internal programming interface, not an HTTP endpoint or external web API.

The communication between modules has one specific purpose: duplicating the data required by another module. When a module creates or updates data that another module needs, it uses the relevant interface to provide that data synchronously. The receiving module stores its own local copy instead of querying the other module's database directly.

This approach keeps every module self-contained. After the required data has been duplicated, each module can perform its own operations using its local data without depending on another module's database or internal implementation.

The interfaces therefore act as controlled synchronization boundaries. They allow data to be shared when necessary while preserving the modularity, ownership, and independent persistence of each module.

### Self-contained modules and duplicated data

To keep modules self-contained, each module maintains its own required copy of the data it uses. The database structure is therefore duplicated between modules rather than shared directly through a single common schema.

This design gives each module clear ownership of its data and prevents tight coupling through shared tables or internal database queries. A module can change its schema, persistence logic, and migrations without requiring other modules to understand or modify its internal storage.

The trade-off is that duplicated data must be kept consistent. When data changes, the responsible module should use the appropriate interface operation so that other modules can update their local copy through the defined contract. This makes ownership explicit and preserves the independence of each module.

## Database structure

The database uses a normalized relational design with four main entities: users, songs, playlists, and playlist-song relationships.

### Users

| Column      | Type     | Description                                    |
| ----------- | -------- | ---------------------------------------------- |
| `Id`        | `Guid`   | Primary key that uniquely identifies the user. |
| `FirstName` | `string` | User's first name.                             |
| `LastName`  | `string` | User's last name.                              |

### Songs

| Column          | Type     | Description                                                        |
| --------------- | -------- | ------------------------------------------------------------------ |
| `Id`            | `Guid`   | Primary key that uniquely identifies the song.                     |
| `PublisherId`   | `Guid`   | Foreign key referencing `Users.Id`. A user can publish many songs. |
| `Name`          | `string` | Song name.                                                         |
| `TimeInSeconds` | `string` | Song duration in seconds.                                          |

The relationship between users and songs is one-to-many: one user can publish multiple songs, while each song belongs to one publisher.

### Playlists

| Column    | Type     | Description                                                            |
| --------- | -------- | ---------------------------------------------------------------------- |
| `Id`      | `Guid`   | Primary key that uniquely identifies the playlist.                     |
| `OwnerId` | `Guid`   | Foreign key referencing `Users.Id`. A user can own multiple playlists. |
| `Name`    | `string` | Playlist name.                                                         |

The relationship between users and playlists is one-to-many: one user can own multiple playlists, while each playlist has one owner.

### PlaylistSongs

| Column       | Type   | Description                             |
| ------------ | ------ | --------------------------------------- |
| `PlaylistId` | `Guid` | Foreign key referencing `Playlists.Id`. |
| `SongId`     | `Guid` | Foreign key referencing `Songs.Id`.     |

`PlaylistSongs` is a junction table that represents the many-to-many relationship between playlists and songs. A playlist can contain many songs, and a song can belong to many playlists. The combined `PlaylistId` and `SongId` columns form a composite primary key, preventing the same song from being added to the same playlist more than once.

## Why this structure is good

- **Clear relationships:** Foreign keys make ownership and publishing relationships explicit and enforce referential integrity.
- **Normalized data:** User, song, playlist, and relationship data are stored separately, reducing duplication and avoiding inconsistent updates within each module.
- **Efficient many-to-many modeling:** The `PlaylistSongs` junction table is the standard relational approach for connecting playlists and songs.
- **Duplicate prevention:** The composite primary key on `PlaylistSongs` prevents duplicate playlist-song associations.
- **Scalable identifiers:** `Guid` primary keys provide globally unique identifiers that work well across distributed services and database environments.
- **Flexible ownership:** Separate `PublisherId` and `OwnerId` fields allow a user to publish songs and own playlists independently.
- **Module independence:** Local copies let modules query and update the data they need without directly accessing another module's database.
- **Simple querying:** The structure supports straightforward queries such as retrieving a user's songs, finding playlists owned by a user, or listing all songs in a playlist.

For production use, `TimeInSeconds` would generally be better stored as an integer type such as `int` rather than `string`. This makes duration sorting, filtering, validation, and arithmetic operations more reliable.
