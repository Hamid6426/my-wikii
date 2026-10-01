# Build / Buildx

Tags: `build` `buildkit` `cli` `containers` `docker` `oci`

**BuildKit** is Docker's modern build engine. **`docker buildx`** is the CLI for advanced builds: multi-platform images, better caching, and exporting to registries.

## Basics

```bash
docker build -t myapp:dev .
# same family, Buildx-aware:

docker buildx build -t myapp:dev --load .
docker buildx ls
docker buildx create --name multi --use   # once, for multi-node/multi-arch setups
```

`--load` puts the result into the local image store (single platform). `--push` sends to a registry (typical for multi-arch).

## Multi-platform

```bash
docker buildx build \
  --platform linux/amd64,linux/arm64 \
  -t myuser/myapp:1.0 \
  --push .
```

On Apple Silicon, building `linux/amd64` may use emulation (slower). Prefer native builders in CI for each arch, or a buildx builder that can reach both.

See [Images](../../images/README.md) and [macOS](../../platforms/macos/README.md).

## Cache

```bash
docker buildx build -t myapp:dev --load \
  --cache-from type=registry,ref=myuser/myapp:buildcache \
  --cache-to type=registry,ref=myuser/myapp:buildcache,mode=max .
```

Local cache works by default for repeated Dockerfile layers; registry cache helps CI.

## Useful flags

| Flag                 | Purpose                                             |
| -------------------- | --------------------------------------------------- |
| `--load`             | Import into local `docker images` (one platform)    |
| `--push`             | Push to registry                                    |
| `--platform`         | Target OS/arch list                                 |
| `--build-arg`        | Pass `ARG` into Dockerfile                          |
| `--secret`           | Build secrets (BuildKit) without baking into layers |
| `--target`           | Stop at a named multi-stage target                  |
| `-f Dockerfile.prod` | Alternate Dockerfile                                |

## Related to context size

Huge build contexts slow every build — see [Build context & .dockerignore](../dockerignore/README.md).

## Related

- [Dockerfile](../../dockerfile/README.md)
- [Images](../../images/README.md)
- [Dockerignore](../dockerignore/README.md)
- [CLI index](../README.md)
- [Docker index](../../README.md)
