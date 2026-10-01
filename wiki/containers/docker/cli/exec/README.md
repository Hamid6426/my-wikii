# Exec

Tags: `cli` `containers` `debug` `docker` `exec` `oci`

`docker exec` runs a **new process inside an already running container**. Use it to open a shell, run one-off commands, or poke at a live app — it does not start a second container.

## Basics

```bash
docker exec web whoami
docker exec -it web sh          # or bash if installed
docker exec -it web bash

# Compose

docker compose exec web sh
```

| Flag                       | Meaning                                                   |
| -------------------------- | --------------------------------------------------------- |
| `-i`                       | Keep STDIN open                                           |
| `-t`                       | Allocate a TTY (need both `-it` for an interactive shell) |
| `-u user` / `-u 1000:1000` | Run as that user/uid                                      |
| `-e KEY=val`               | Extra environment for this process                        |
| `-w /path`                 | Working directory                                         |
| `-d`                       | Detached (run in background inside the container)         |

```bash
docker exec -u root -it web sh
docker exec -w /app -it web sh
docker exec -e DEBUG=1 web python manage.py check
```

## Mental model

```
docker run     → starts the container's main process (PID 1 / ENTRYPOINT+CMD)
docker exec    → extra process(es) alongside that main process
docker attach  → reconnect to the main process's stdio (not a new shell)
```

| Command       | Use when                                                                                   |
| ------------- | ------------------------------------------------------------------------------------------ |
| `exec`        | You want a shell or tool next to the app                                                   |
| `attach`      | You want the original foreground process I/O (easy to signal-kill the app — prefer `exec`) |
| `compose run` | One-off **new** container from a service definition (migrations, etc.)                     |

## Interactive shells

Many slim images only have `sh` (Alpine, distroless often have **no shell at all**):

```bash
docker exec -it web sh
# if bash exists:

docker exec -it web bash
```

Distroless / scratch images: `exec` into a shell usually **fails** — debug with a sidecar, ephemeral debug container sharing namespaces, or rebuild a debug variant.

## Common tasks

```bash
# process list inside the container's PID namespace

docker exec web ps aux

# env seen by a new process (not always identical to PID 1's original env)

docker exec web env

# quick HTTP check from inside the network namespace

docker exec web wget -qO- http://127.0.0.1:8080/health

# package install for debugging (ephemeral — lost when container is recreated)

docker exec -u root web apt-get update && docker exec -u root web apt-get install -y curl
```

Prefer fixing the [image](../../images/README.md) / [Dockerfile](../../dockerfile/README.md) over "exec and apt-get" as a habit.

## Gotchas

| Gotcha                        | Detail                                                                      |
| ----------------------------- | --------------------------------------------------------------------------- |
| Container must be **running** | `exec` fails on stopped containers — use `docker start` or `docker run`     |
| Not the same as PID 1         | Crashing your exec shell does not stop the app; killing PID 1 does          |
| User / permissions            | May need `-u root` for package installs; production images may forbid root  |
| No TTY in CI                  | Drop `-t` when stdin is not a terminal: `docker exec -i web sh < script.sh` |
| Compose service name          | `docker compose exec web` — service name, not always the container name     |

## Security note

`exec` as root into a production container is a break-glass tool. Anyone who can `exec` (or use the Docker socket) can often reach host-level power — see [Security](../../security/README.md).

## Related

- [Logging](../logging/README.md)
- [CLI index](../README.md)
- [Containers](../../containers/README.md)
- [Compose](../compose/README.md)
- [Docker index](../../README.md)
