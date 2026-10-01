# Images and registries

Tags: `containers` `docker` `images` `oci` `registry`

An **image** is an immutable, layered template. A **registry** stores images so you can `pull` / `push` them across machines.

## Image names

```
[registry/][namespace/]repository[:tag|@digest]
```

Examples:

| Reference                | Meaning                                |
| ------------------------ | -------------------------------------- |
| `nginx:alpine`           | Docker Hub library image, tag `alpine` |
| `ghcr.io/acme/api:1.4.2` | GitHub Container Registry              |
| `nginx@sha256:…`         | Pin by content digest (immutable)      |

Avoid relying on floating tags like `:latest` in production — prefer version tags or digests.

## Everyday image commands

```bash
docker pull nginx:alpine
docker images
# or: docker image ls

docker tag nginx:alpine myregistry.example/nginx:alpine
docker push myregistry.example/nginx:alpine

docker rmi nginx:alpine
docker image prune    # dangling layers
docker image prune -a # unused images (careful)
```

## Layers

Each Dockerfile instruction typically adds a layer. Shared layers between images save disk and pull time. Inspect:

```bash
docker history myapp:dev
docker inspect myapp:dev
```

## Registries

| Registry            | Notes                             |
| ------------------- | --------------------------------- |
| **Docker Hub**      | Default when no host is specified |
| **GHCR**            | `ghcr.io` — GitHub packages       |
| **ECR / ACR / GCR** | Cloud provider registries         |
| **Self-hosted**     | Harbor, distribution, etc.        |

```bash
docker login
docker login ghcr.io
```

Private pulls need credentials (Desktop keychain helpers, `credHelpers`, CI secrets).

## Multi-arch images

One tag can point at a **manifest list** with `amd64`, `arm64`, etc. Useful for Apple Silicon + servers:

```bash
docker buildx build --platform linux/amd64,linux/arm64 -t myuser/app:1.0 --push .
docker buildx imagetools inspect myuser/app:1.0
```

See also platform notes under [Platforms](../platforms/README.md).

## Save / load (air-gapped)

```bash
docker save myapp:dev -o myapp.tar
docker load -i myapp.tar
```

## Related

- [Dockerfile](../dockerfile/README.md)
- [CLI](../cli/README.md)
- [Security](../security/README.md)
- [Docker index](../README.md)
