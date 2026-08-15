# Leftborn Task

## Run with Docker Compose

The easiest way to run Leftborn Task locally is with Docker Compose.

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
