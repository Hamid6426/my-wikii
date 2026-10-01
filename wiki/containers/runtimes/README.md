# Runtimes

Tags: `containers` `oci` `runtime` `runtimes`

Low-level **container runtimes** — what actually starts container processes. Engines (Docker, Podman) and Kubernetes sit above these.

## Pages

| Runtime        | Role                                                                   | Page                               |
| -------------- | ---------------------------------------------------------------------- | ---------------------------------- |
| **containerd** | Industry-standard daemon; Docker uses it under the hood; common in K8s | [containerd](containerd/README.md) |
| **CRI-O**      | K8s-focused OCI runtime stack (CRI implementation)                     | [cri-o](cri-o/README.md)           |

Also hear about: **runc** / **crun** (OCI runtime execs), **LXC/LXD** (system containers — different use case than app containers).

## When you care

- Running/administering **Kubernetes** nodes
- Replacing Docker Engine with a thinner stack (`nerdctl` + containerd)
- Understanding what Desktop/Engine is wrapping

For day-to-day app dev, you usually stay at the [Docker](../docker/README.md) or [Podman](../podman/README.md) layer.

## Related

- [Containers index](../README.md)
- [Image builders](../image-builders/README.md)
- [Docker](../docker/README.md)
