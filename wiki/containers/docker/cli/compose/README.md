# Compose

Tags: `cli` `compose` `containers` `docker` `oci` `orchestration`

**Docker Compose** is both a **file format** (`compose.yaml`) that describes a multi-container app and a **CLI** (`docker compose …`) that runs it. This page lives under [CLI](../README.md) because you drive it from the command line, but the YAML model matters as much as the commands.

Use the **v2 plugin**: `docker compose` (space), not the legacy `docker-compose` binary when possible.

## Minimal file

`compose.yaml` (or `docker-compose.yml`):

```yaml
services:
  web:
    build: .
    ports:
      - "8000:8000"
    environment:
      DATABASE_URL: postgres://app:app@db:5432/app
    depends_on:
      - db
  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_USER: app
      POSTGRES_PASSWORD: app
      POSTGRES_DB: app
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

```bash
docker compose up -d
docker compose ps
docker compose logs -f web
docker compose down        # stop & remove containers/network
docker compose down -v     # also remove named volumes
```

## Core concepts

| Key                        | Role                                                                     |
| -------------------------- | ------------------------------------------------------------------------ |
| `services`                 | Containers to run (one or more)                                          |
| `build`                    | Build from a [Dockerfile](../../dockerfile/README.md) context            |
| `image`                    | Run a prebuilt image                                                     |
| `ports`                    | Publish `host:container`                                                 |
| `volumes`                  | Mounts — see [Volumes](../../volumes/README.md)                          |
| `networks`                 | Custom nets — see [Networking](../../networking/README.md)               |
| `environment` / `env_file` | Config inside the container                                              |
| `depends_on`               | Start order (not a full readiness wait unless you add health conditions) |
| `profiles`                 | Optional groups of services (`--profile`)                                |

Services on the default Compose network can reach each other by **service name** (DNS), e.g. host `db` from `web`.

## Useful commands

```bash
docker compose build
docker compose up -d --build
docker compose exec web sh
docker compose run --rm web python manage.py migrate
docker compose config          # print resolved YAML
docker compose watch           # (when configured) sync/rebuild on change
```

## Overrides & env

| File                    | Typical use                                    |
| ----------------------- | ---------------------------------------------- |
| `compose.yaml`          | Base definition                                |
| `compose.override.yaml` | Local dev overrides (auto-merged when present) |
| `.env`                  | Interpolate `${VAR}` in the Compose file       |

```yaml
# compose.yaml fragment

services:
  web:
    ports:
      - "${HOST_PORT:-8000}:8000"
```

## Healthchecks & depends_on

```yaml
services:
  db:
    image: postgres:16-alpine
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U app"]
      interval: 5s
      retries: 5
  web:
    depends_on:
      db:
        condition: service_healthy
```

## Related

- [Networking](../../networking/README.md)
- [Volumes](../../volumes/README.md)
- [Dockerfile](../../dockerfile/README.md)
- [CLI index](../README.md)
- [Exec](../exec/README.md)
- [Logging](../logging/README.md)
- [Docker index](../../README.md)
