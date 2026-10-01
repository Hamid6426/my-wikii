# Security

Tags: `containers` `docker` `oci` `security`

Docker isolation is strong for many workloads, but **misconfiguration is easy**. Treat the Engine socket and privileged containers as high trust.

## Mental model

| Risk                             | Why                                                                                   |
| -------------------------------- | ------------------------------------------------------------------------------------- |
| Access to `docker.sock`          | Effectively **root on the host** (mount a container with the socket → control Engine) |
| Running as root in the container | Breakout bugs are worse; write host-owned files as root via binds                     |
| Fat images                       | More packages → more CVEs                                                             |
| Secrets in images/env            | `ENV PASSWORD=` and layers are visible via history/inspect                            |
| Privileged / host mounts         | `--privileged`, `/var/run/docker.sock`, host `/` binds erase isolation                |

## Practical defaults

1. **Non-root USER** in the [Dockerfile](../dockerfile/README.md) when the app allows it.
2. Prefer **slim/distroless** bases; multi-stage builds.
3. Don't put secrets in images — use env at runtime, Compose `secrets`, or an external secret manager.
4. Never expose Docker socket to untrusted containers.
5. Publish ports on `127.0.0.1` when only local access is needed.
6. Pin image versions or digests in production — [Images](../images/README.md).

```dockerfile
RUN useradd -r -u 10001 app
USER app
```

## Rootless Engine

On Linux, **rootless Docker** runs the daemon as a user — smaller blast radius, with feature limits (some ports, storage, overlays). See [Docker on Linux](../platforms/linux/README.md).

## Scanning & updates

```bash
docker scout quickview myapp:dev   # if Scout available
# or use Trivy / Grype / registry scanners in CI

```

Rebuild and redeploy regularly when base images get security updates.

## Compose secrets (file-based)

```yaml
services:
  web:
    secrets:
      - db_password
secrets:
  db_password:
    file: ./secrets/db_password.txt
```

Still protect the files on disk and in CI; this only avoids baking secrets into the image.

## Hardening checklist

- [ ] Non-root user
- [ ] Read-only root FS where possible (`--read-only` + tmpfs for writable paths)
- [ ] Drop capabilities (`--cap-drop=ALL` + add back only what you need)
- [ ] No `--privileged` unless you truly need it
- [ ] Resource limits (`--memory`, `--cpus`)
- [ ] Minimal published ports
- [ ] CI image scan gate

## Related

- [Dockerfile](../dockerfile/README.md)
- [Images](../images/README.md)
- [Volumes](../volumes/README.md)
- [Platforms](../platforms/README.md)
- [Docker index](../README.md)
