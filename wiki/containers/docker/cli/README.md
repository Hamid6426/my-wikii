# CLI

Tags: `cli` `containers` `docker` `oci`

The `docker` client talks to the Engine (`dockerd`) over a socket or TCP context. This section is the command-line surface: everyday cookbook plus deeper command-oriented pages.

## Pages

| Page                                   | Focus                                                      |
| -------------------------------------- | ---------------------------------------------------------- |
| [Compose](compose/README.md)           | YAML model + `docker compose` CLI                          |
| [Exec](exec/README.md)                 | `docker exec` — shells and commands in a running container |
| [Logging](logging/README.md)           | `docker logs`, drivers, rotation                           |
| [Build / Buildx](build/README.md)      | BuildKit, multi-arch, cache                                |
| [Dockerignore](dockerignore/README.md) | Build context and `.dockerignore`                          |
| [Healthchecks](healthchecks/README.md) | `HEALTHCHECK`, Compose readiness                           |
| [Contexts](contexts/README.md)         | Remote Docker / `docker context`                           |

## Orientation

```bash
docker version          # client + server
docker info             # engine config, storage, cgroup, …
docker context ls       # local Desktop, remote engines, …
```

## Images

```bash
docker pull IMAGE
docker images
docker build -t NAME:TAG .
docker buildx build --platform linux/amd64 -t NAME:TAG --load .
docker rmi NAME:TAG
docker image prune
```

More: [Images](../images/README.md), [Dockerfile](../dockerfile/README.md).

## Containers lifecycle

```bash
docker run -d --name web -p 8080:80 nginx:alpine
docker ps
docker ps -a
docker stop web
docker start web
docker restart web
docker rm web
docker rm -f web          # force remove running
docker run --rm …         # auto-remove on exit
```

Useful `run` flags:

| Flag                       | Purpose                                               |
| -------------------------- | ----------------------------------------------------- |
| `-d`                       | Detached                                              |
| `-it`                      | Interactive TTY                                       |
| `--name`                   | Stable name                                           |
| `-e KEY=val`               | Env                                                   |
| `-v` / `--mount`           | Volumes — [Volumes](../volumes/README.md)             |
| `-p`                       | Publish ports — [Networking](../networking/README.md) |
| `--restart unless-stopped` | Restart policy                                        |
| `--memory` / `--cpus`      | Limits                                                |

## Logs, exec, copy

```bash
docker logs web
docker logs -f --tail 100 web
docker exec -it web sh
docker cp web:/etc/nginx/nginx.conf ./nginx.conf
docker cp ./file web:/tmp/file
```

More: [Logging](logging/README.md), [Exec](exec/README.md).

## Inspect & debug

```bash
docker inspect web
docker stats
docker top web
docker events
docker diff web           # filesystem changes vs image
echo $?                   # after run — exit code
docker run --rm IMAGE true; echo $?
```

## Cleanup

```bash
docker container prune
docker image prune
docker volume prune
docker network prune
docker system prune       # containers + networks + dangling images
docker system prune -a --volumes   # nuclear; know what you lose
docker system df          # disk usage breakdown
```

## Compose shortcuts

```bash
docker compose up -d
docker compose down
docker compose logs -f
docker compose exec SERVICE sh
```

See [Compose](compose/README.md).

## Related

- [Networking](../networking/README.md)
- [Security](../security/README.md)
- [Containers](../containers/README.md)
- [Docker index](../README.md)
