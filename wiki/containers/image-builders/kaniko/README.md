# Kaniko

Tags: `build` `ci` `containers` `image-builders` `kaniko` `kubernetes` `oci`

**Kaniko** builds container images from a Dockerfile **inside a Kubernetes cluster** (or similarly restricted environment) without mounting the Docker socket or running a privileged Docker daemon.

Typical use: CI job as a Pod that builds and pushes to a registry. Not a local Desktop replacement.

## Related

- [Image builders index](../README.md)
- [containerd](../../runtimes/containerd/README.md)
- [Docker Buildx](../../docker/cli/build/README.md) — local/CI multi-arch alternative
- [Docker security](../../docker/security/README.md) — why avoiding the socket matters
