# Healthchecks

Tags: `cli` `containers` `docker` `healthchecks` `oci` `ops` `reliability`

A **healthcheck** is a command Docker runs inside a container to decide if it is **healthy**, **unhealthy**, or still **starting**. Useful for orchestration, Compose `depends_on` conditions, and load balancers that honor Docker health state.

## Dockerfile

```dockerfile
HEALTHCHECK --interval=10s --timeout=3s --start-period=20s --retries=3 \
  CMD curl -fsS http://127.0.0.1:8080/health || exit 1
```

| Option              | Meaning                                         |
| ------------------- | ----------------------------------------------- |
| `interval`          | Time between checks                             |
| `timeout`           | Max time for one check                          |
| `start-period`      | Grace period after start (failures don't count) |
| `retries`           | Failures before `unhealthy`                     |
| `CMD` / `CMD-SHELL` | Probe command (`exit 0` = healthy)              |

Disable inherited healthcheck: `HEALTHCHECK NONE`.

## Runtime / Compose

```bash
docker run -d --name web \
  --health-cmd="curl -fsS http://127.0.0.1/ || exit 1" \
  --health-interval=10s \
  nginx:alpine

docker inspect --format='{{.State.Health.Status}}' web
```

```yaml
services:
  db:
    image: postgres:16-alpine
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U app"]
      interval: 5s
      timeout: 5s
      retries: 10
  web:
    depends_on:
      db:
        condition: service_healthy
```

Without `condition: service_healthy`, `depends_on` only waits for the container to **start**, not to be ready.

## Design tips

- Probe the real readiness path (DB accept connections, HTTP `/health` that checks deps carefully).
- Don't make `/health` so heavy it DDOSes yourself.
- Slim images may lack `curl` — use `wget`, a tiny static probe, or `CMD` that calls the app binary.
- Health ≠ "process running"; a wedged app can still have PID 1 alive.

## Related

- [Containers](../../containers/README.md)
- [Compose](../compose/README.md)
- [Troubleshooting](../../troubleshooting/README.md)
- [CLI index](../README.md)
