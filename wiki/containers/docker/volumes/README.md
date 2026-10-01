# Volumes

Tags: `containers` `docker` `oci` `persistence` `storage` `volumes`

Containers are disposable; **data you care about** should live in volumes or bind mounts, not only the writable container layer.

## Three mount types

| Type             | Declared as                         | Persists after remove?  | Best for                    |
| ---------------- | ----------------------------------- | ----------------------- | --------------------------- |
| **Named volume** | `volname:/path`                     | Yes (until `volume rm`) | Databases, durable app data |
| **Bind mount**   | `/host/path:/path` or `./dir:/path` | Yes (host files)        | Live code in dev            |
| **tmpfs**        | `tmpfs: /path`                      | No (RAM)                | Secrets scratch, caches     |

```bash
docker volume ls
docker volume create pgdata
docker volume inspect pgdata
docker volume rm pgdata
```

## Run examples

```bash
# named volume

docker run -d --name db -v pgdata:/var/lib/postgresql/data postgres:16-alpine

# bind mount (dev)

docker run --rm -v "$(pwd)":/app -w /app node:22 npm test

# tmpfs

docker run --rm --tmpfs /tmp:rw,size=64m alpine
```

## Compose

```yaml
services:
  db:
    image: postgres:16-alpine
    volumes:
      - pgdata:/var/lib/postgresql/data
      - ./init:/docker-entrypoint-initdb.d:ro
  web:
    volumes:
      - ./src:/app/src # bind for hot reload

volumes:
  pgdata:
```

## Permissions

Processes inside Linux containers run as a **UID/GID**. Bind-mounted files on the host must be readable/writable by that user — common friction on Linux. Named volumes are initialized with the image's ownership more often "just work" for official DB images.

On [Windows](../platforms/windows/README.md) / [macOS](../platforms/macos/README.md), bind performance and ownership differ (WSL path vs `/mnt/c`, Desktop file sharing). Prefer WSL filesystem on Windows; prefer named volumes for databases on Mac.

## Backup sketch

```bash
docker run --rm -v pgdata:/data -v "$(pwd)":/backup alpine \
  tar czf /backup/pgdata.tgz -C /data .
```

## Cleanup

```bash
docker volume prune   # unused volumes only
```

`docker compose down -v` removes volumes declared in that project's Compose file — destructive; know before you run it.

## Related

- [Compose](../cli/compose/README.md)
- [Networking](../networking/README.md)
- [Security](../security/README.md)
- [Docker index](../README.md)
