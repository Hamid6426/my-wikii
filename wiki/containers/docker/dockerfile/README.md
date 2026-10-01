# Dockerfile

Tags: `build` `containers` `docker` `dockerfile` `images` `oci`

A **Dockerfile** is a text recipe Docker uses to **build an image**. Each instruction usually creates a layer; layer caching makes rebuilds faster when early steps do not change.

## Minimal example

```dockerfile
FROM python:3.12-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
EXPOSE 8000
CMD ["python", "main.py"]
```

```bash
docker build -t myapp:dev .
docker run --rm -p 8000:8000 myapp:dev
```

## Common instructions

| Instruction    | Role                                                                |
| -------------- | ------------------------------------------------------------------- |
| `FROM`         | Base image (start of a stage)                                       |
| `WORKDIR`      | Set working directory (creates if missing)                          |
| `COPY` / `ADD` | Copy files into the image (`COPY` preferred; `ADD` has extra magic) |
| `RUN`          | Execute build-time commands (install packages, compile)             |
| `ENV`          | Environment variables in the image                                  |
| `ARG`          | Build-time variables (`docker build --build-arg`)                   |
| `EXPOSE`       | Documents ports (does not publish them)                             |
| `USER`         | Switch to non-root user                                             |
| `CMD`          | Default command when the container starts                           |
| `ENTRYPOINT`   | Fixed executable; `CMD` often supplies default args                 |
| `HEALTHCHECK`  | Optional command Docker runs to mark healthy/unhealthy              |

## Layer caching tips

Order from **least → most frequently changing**:

1. Base image + system packages
2. Dependency manifests (`package.json`, `requirements.txt`, `go.mod`, …)
3. `RUN` install dependencies
4. Application source (`COPY . .`)

Use a **`.dockerignore`** so `node_modules`, `.git`, build artifacts, and secrets never enter the build context — details in [Build context & .dockerignore](../cli/dockerignore/README.md).

```gitignore
.git
node_modules
**/__pycache__
.env
*.md
```

## Multi-stage builds

Build with a fat toolchain image, copy only the artifact into a slim runtime image:

```dockerfile
FROM golang:1.22 AS build
WORKDIR /src
COPY go.mod go.sum ./
RUN go mod download
COPY . .
RUN CGO_ENABLED=0 go build -o /out/app

FROM gcr.io/distroless/static:nonroot
COPY --from=build /out/app /app
USER nonroot:nonroot
ENTRYPOINT ["/app"]
```

Benefits: smaller images, fewer CVEs in the final image, no compilers in production.

## CMD vs ENTRYPOINT

| Pattern                                 | Behavior                                                               |
| --------------------------------------- | ---------------------------------------------------------------------- |
| `CMD ["app"]` only                      | Easy to override: `docker run img other-cmd`                           |
| `ENTRYPOINT ["app"]` + `CMD ["--flag"]` | `app` always runs; args replace `CMD`                                  |
| Shell form `CMD app`                    | Runs via `/bin/sh -c` (signal quirks) — prefer exec form `CMD ["app"]` |

## Best practices (short list)

- Pin base tags thoughtfully (`python:3.12.6-slim` or digests in prod).
- Run as non-root (`USER`) when possible — see [Security](../security/README.md).
- One process mindset per container (logs to stdout/stderr).
- Prefer `COPY` over `ADD` unless you need tar auto-extract/URL fetch.
- Combine related `RUN` steps carefully; don't fight the cache blindly.

## Related

- [Images & registries](../images/README.md)
- [Build / Buildx](../cli/build/README.md)
- [Dockerignore](../cli/dockerignore/README.md)
- [Compose](../cli/compose/README.md)
- [Security](../security/README.md)
- [Docker index](../README.md)
