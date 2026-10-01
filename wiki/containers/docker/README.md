# Docker

Tags: `containers` `docker` `engine` `oci`

Docker packages apps and their dependencies into **images**, then runs them as isolated **containers**. Same image can run on a laptop, CI, or a server — with platform caveats (especially Windows containers vs Linux containers).

## Core ideas

| Term           | Meaning                                             |
| -------------- | --------------------------------------------------- |
| **Image**      | Immutable template (layers): app + libs + config    |
| **Container**  | Running (or stopped) instance of an image           |
| **Dockerfile** | Recipe to build an image                            |
| **Registry**   | Image store (Docker Hub, GHCR, private registries)  |
| **Compose**    | YAML to run multi-container apps (`docker compose`) |
| **Volume**     | Persistent data outside the container filesystem    |
| **Network**    | How containers talk to each other and the host      |

```
Dockerfile → build → Image → run → Container
                      ↑
                 pull from registry
```

## Mentally model the host OS

Docker needs a **Linux kernel** for Linux containers (the common case). How you get that differs by OS — see [Platforms](platforms/README.md):

| Host                                   | How Docker usually runs                                                          |
| -------------------------------------- | -------------------------------------------------------------------------------- |
| [Linux](platforms/linux/README.md)     | Engine runs natively on the host kernel                                          |
| [Windows](platforms/windows/README.md) | Docker Desktop + WSL2 (Linux containers) or Hyper-V; optional Windows containers |
| [macOS](platforms/macos/README.md)     | Docker Desktop + a Linux VM (Apple Virtualization / HyperKit lineage)            |

## Everyday commands

```bash
docker version
docker info

docker pull nginx:alpine
docker images
docker run --rm -p 8080:80 nginx:alpine

docker ps
docker ps -a
docker logs <container>
docker exec -it <container> sh

docker build -t myapp:dev .
docker compose up -d
docker compose down
```

## Dockerfile sketch

```dockerfile
FROM python:3.12-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
CMD ["python", "main.py"]
```

## Compose sketch

```yaml
services:
  web:
    build: .
    ports:
      - "8000:8000"
    volumes:
      - ./data:/data
  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_PASSWORD: example
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

## Pages in this section

| Page                                         | Focus                                                                                                                    |
| -------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| [Platforms](platforms/README.md)             | Docker on [Linux](platforms/linux/README.md), [Windows](platforms/windows/README.md), [macOS](platforms/macos/README.md) |
| [Dockerfile](dockerfile/README.md)           | Build recipes, layers, multi-stage                                                                                       |
| [Images](images/README.md)                   | Tags, registries, digests, multi-arch                                                                                    |
| [Containers](containers/README.md)           | Lifecycle, restart policies, limits                                                                                      |
| [Volumes](volumes/README.md)                 | Named volumes, binds, tmpfs                                                                                              |
| [Networking](networking/README.md)           | Bridge, ports, Compose DNS & recipes                                                                                     |
| [Security](security/README.md)               | Socket risk, non-root, secrets, scanning                                                                                 |
| [CLI](cli/README.md)                         | Cookbook, Compose, exec, logs, buildx, healthchecks, contexts                                                            |
| [Dev Containers](devcontainers/README.md)    | IDE development inside containers                                                                                        |
| [Troubleshooting](troubleshooting/README.md) | Pull/port/socket/disk/platform failures                                                                                  |

## Related

- Parent topic: [Containers](../README.md) — Podman, runtimes, builders, desktop GUIs
- [Podman](../podman/README.md) · [Desktop interfaces](../desktop-interfaces/README.md)
- Linux kernel — [linux-kernel](../../linux/linux-kernel/README.md)
- Distros — [Distros](../../linux/distros/README.md) · [package managers](../../linux/package-managers/README.md)
- [Ubuntu](../../linux/distros/ubuntu/README.md) / [Fedora](../../linux/distros/fedora/README.md)
