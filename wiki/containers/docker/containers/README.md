# Containers

Tags: `containers` `docker` `lifecycle` `oci`

A **container** is a runnable instance of an [image](../images/README.md): isolated processes with their own filesystem view, network interfaces, and resource controls — sharing the host kernel.

## Lifecycle

```
create → start → running ⇄ paused
                 ↓
               stopped → remove
```

```bash
docker create --name web nginx:alpine
docker start web
docker stop web
docker kill web          # SIGKILL
docker pause web / docker unpause web
docker rm web
```

`docker run` = create + start (plus attach/log options).

## Restart policies

| Policy           | Behavior                                       |
| ---------------- | ---------------------------------------------- |
| `no`             | Default — don't restart                        |
| `on-failure[:n]` | Restart on non-zero exit                       |
| `always`         | Always restart (including after daemon reboot) |
| `unless-stopped` | Like always, unless you explicitly stopped it  |

```bash
docker run -d --restart unless-stopped --name api myapi:1.0
```

## Healthchecks

Defined in the [Dockerfile](../dockerfile/README.md) or at runtime / Compose. Docker marks the container healthy/unhealthy; orchestrators and `depends_on` conditions can wait on that.

```bash
docker inspect --format='{{.State.Health.Status}}' web
```

## Resource limits

```bash
docker run -d --memory=512m --cpus=1.5 --name api myapi:1.0
docker stats
```

Without limits, a container can consume most of the host (noisy neighbor).

## Init and signals

Prefer **exec-form** `CMD`/`ENTRYPOINT` so PID 1 receives signals correctly. For shell-heavy images, `--init` (tini) helps reap zombies and forward signals:

```bash
docker run --init --rm myapp:dev
```

## Ephemeral by default

The writable layer is discarded with `docker rm` unless you keep data in [volumes](../volumes/README.md). Treat containers as cattle: rebuild/replace instead of snowflake patching.

## Related

- [Images](../images/README.md)
- [CLI](../cli/README.md)
- [Volumes](../volumes/README.md)
- [Networking](../networking/README.md)
- [Compose](../cli/compose/README.md)
- [Docker index](../README.md)
