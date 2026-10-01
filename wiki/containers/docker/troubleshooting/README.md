# Troubleshooting

Tags: `containers` `debug` `docker` `oci` `ops` `troubleshooting`

Quick checks when Docker misbehaves. Confirm **context** and **engine** first:

```bash
docker context show
docker info
docker version
```

## Can't pull / push

| Check                 | What to try                                                    |
| --------------------- | -------------------------------------------------------------- |
| Network / VPN / proxy | Disable VPN test; set proxy env; Desktop → Resources → Proxies |
| Auth                  | `docker login` / `docker logout`; expired token                |
| Rate limits           | Hub anonymous limits — login or mirror                         |
| DNS                   | `docker run --rm busybox nslookup registry-1.docker.io`        |
| Clock skew            | TLS fails if system time is wrong                              |

## Platform / arch mismatch

```
exec format error
```

Image arch ≠ host (e.g. `amd64` on ARM without emulation). Use `--platform`, multi-arch images, or rebuild — [Buildx](../cli/build/README.md), [macOS](../platforms/macos/README.md).

## Port already allocated

```bash
docker ps --format '{{.Names}} {{.Ports}}'
# Linux: sudo ss -ltnp | grep :8080

```

Stop the other container or change `-p`.

## Permission denied / socket

```
permission denied while trying to connect to the Docker daemon socket
```

On Linux: user not in `docker` group, or daemon down — [Linux platform](../platforms/linux/README.md). On Desktop: start Docker Desktop.

## Disk full / huge log files

```bash
docker system df
docker system prune
```

Enable log rotation — [Logging](../cli/logging/README.md).

## Bind mounts slow or empty (Mac / Windows)

- Windows: project under WSL home, not `/mnt/c` — [Windows](../platforms/windows/README.md)
- Mac: prefer named volumes for DBs; check file sharing — [macOS](../platforms/macos/README.md)
- Typo in host path → empty directory mounted

## Container exits immediately

```bash
docker ps -a
docker logs <name>
docker inspect --format='{{.State.ExitCode}}' <name>
```

Main process ended (bad CMD, missing config, DB not ready). Add [healthchecks](../cli/healthchecks/README.md) / fix entrypoint.

## Compose "started but not ready"

`depends_on` without `condition: service_healthy` only waits for start — [Compose](../cli/compose/README.md), [Healthchecks](../cli/healthchecks/README.md).

## Build context huge / slow

Add `.dockerignore` — [Dockerignore](../cli/dockerignore/README.md).

## Related

- [CLI](../cli/README.md)
- [Logging](../cli/logging/README.md)
- [Exec](../cli/exec/README.md)
- [Platforms](../platforms/README.md)
- [Security](../security/README.md)
- [Docker index](../README.md)
