# Logging

Tags: `cli` `containers` `docker` `logging` `oci` `ops`

Docker's default logging model: the app writes to **stdout / stderr**, and the Engine's **logging driver** captures that stream so you can read it with `docker logs`.

## App side (best practice)

- Log to **stdout/stderr**, not a random file inside the container (files vanish with the container unless on a [volume](../../volumes/README.md)).
- One log line per event when possible; JSON logs play well with aggregators.
- Don't rely on `docker exec` + `tail /var/log/...` as the primary workflow.

```dockerfile
# good: process in foreground, logs to stdout

CMD ["nginx", "-g", "daemon off;"]
```

## `docker logs`

```bash
docker logs web
docker logs -f web                 # follow (like tail -f)
docker logs --tail 100 web
docker logs --since 30m web
docker logs --until 2026-08-08T12:00:00 web
docker logs -t web                 # show timestamps
docker logs --details web          # env/label extras when present
```

Compose:

```bash
docker compose logs
docker compose logs -f web
docker compose logs --tail=50 db
```

Only works for drivers that support reading back through the Engine API (notably the default **json-file** and **local** drivers). Some drivers ship logs elsewhere only (e.g. certain syslog/fluentd setups).

## Logging drivers

Configured on the daemon and/or per container:

```bash
docker info | grep -i logging
docker inspect -f '{{.HostConfig.LogConfig.Type}}' web
```

| Driver                                                   | Notes                                                                         |
| -------------------------------------------------------- | ----------------------------------------------------------------------------- |
| **json-file**                                            | Common Desktop/Engine default; stored on disk as JSON; supports `docker logs` |
| **local**                                                | Efficient local buffer; supports `docker logs`                                |
| **journald**                                             | Sends to systemd journal (Linux)                                              |
| **syslog** / **fluentd** / **awslogs** / **gcplogs** / … | Ship to external systems                                                      |

```bash
docker run -d --name web \
  --log-driver json-file \
  --log-opt max-size=10m \
  --log-opt max-file=3 \
  nginx:alpine
```

Daemon-wide defaults live in `daemon.json` (path differs by [platform](../../platforms/README.md)):

```json
{
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "10m",
    "max-file": "3"
  }
}
```

Without rotation (`max-size` / `max-file`), busy containers can **fill the disk**.

## Where the files live (json-file)

On Linux Engine, roughly:

```text
/var/lib/docker/containers/<container-id>/<container-id>-json.log
```

Prefer `docker logs` over editing those files by hand. Disk usage:

```bash
docker system df
du -sh /var/lib/docker/containers/*/*-json.log 2>/dev/null | sort -h
```

## Follow vs exec

| Goal                                   | Prefer                                                                             |
| -------------------------------------- | ---------------------------------------------------------------------------------- |
| See the app's stdout/stderr history    | `docker logs` / `compose logs`                                                     |
| Interactive shell inside the container | [`docker exec`](../exec/README.md)                                                 |
| App logs only to a file on a volume    | `exec` + `tail`, or mount and read from host — better to fix the app to use stdout |

## Production patterns

- **Rotation** on json-file/local (`max-size`, `max-file`).
- Ship logs to a stack (Loki, ELK, CloudWatch, …) via a driver or a **DaemonSet/agent** that tails Docker/containerd logs.
- Keep sensitive data out of logs (tokens, PII).
- Correlate with `docker events` and healthchecks when debugging restarts — [Containers](../../containers/README.md), [CLI](../README.md).

## Related

- [Exec](../exec/README.md)
- [CLI index](../README.md)
- [Containers](../../containers/README.md)
- [Compose](../compose/README.md)
- [Docker index](../../README.md)
