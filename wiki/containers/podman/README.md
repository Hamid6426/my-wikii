# Podman

Tags: `containers` `daemonless` `engine` `oci` `podman`

**Podman** is an OCI-focused container engine with a Docker-compatible CLI. It is **daemonless** by default (no long-running root `dockerd`) and strong on **rootless** workflows — especially common on Fedora / RHEL-family systems.

## Mental model

```
podman CLI  →  forks runtime (crun/runc) + storage/network setup
             (no central privileged daemon required for typical use)
```

```bash
podman version
podman info
podman run --rm -p 8080:80 nginx:alpine
podman build -t myapp:dev .
podman ps -a
```

Many scripts work with `alias docker=podman` or the `podman-docker` package that provides a `docker` shim — verify Compose and edge flags; compatibility is high but not 100%.

## Pages

| Page                                       | Focus                                     |
| ------------------------------------------ | ----------------------------------------- |
| [Rootless](rootless/README.md)             | User namespaces, ports, lingering         |
| [Podman Desktop](podman-desktop/README.md) | GUI; also listed under desktop interfaces |
| [Compose](compose/README.md)               | `podman compose` / compose compatibility  |

## vs Docker (short)

| Topic         | Podman                         | Docker Engine                       |
| ------------- | ------------------------------ | ----------------------------------- |
| Daemon        | Typically none                 | `dockerd`                           |
| Rootless      | First-class                    | Optional / more setup               |
| Desktop       | Podman Desktop                 | Docker Desktop (licensing)          |
| K8s adjacency | pods, `generate kube`, Quadlet | Compose-first culture; Swarm legacy |

Prefer Podman when you want rootless local engines on Linux or to avoid Docker Desktop licensing. Prefer Docker when team docs/CI assume Desktop + exact Docker Compose behavior.

## Related

- [Containers index](../README.md)
- [Docker](../docker/README.md)
- [Desktop interfaces](../desktop-interfaces/README.md)
- [Runtimes](../runtimes/README.md)
- [Buildah](../image-builders/buildah/README.md) — often paired for builds
- [Fedora](../../linux/distros/fedora/README.md)
