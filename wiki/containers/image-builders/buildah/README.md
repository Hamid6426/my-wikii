# Buildah

Tags: `build` `buildah` `containers` `daemonless` `image-builders` `oci`

**Buildah** builds OCI images without a Docker daemon. Often installed alongside [Podman](../../podman/README.md); `podman build` uses Buildah under the hood on many setups.

```bash
buildah from alpine
buildah run <container> -- apk add --no-cache curl
buildah copy <container> ./app /app
buildah commit <container> myapp:dev
```

Also understands Dockerfile-oriented workflows via `buildah bud` / Podman build.

## Related

- [Image builders index](../README.md)
- [Podman](../../podman/README.md)
- [Docker Dockerfile](../../docker/dockerfile/README.md)
