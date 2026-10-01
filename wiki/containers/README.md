# Containers

Tags: `containers` `index` `oci`

Containerization notes: engines, desktops, image builders, and orchestration-adjacent tools. **Docker** is the deepest section here; peers cover drop-in alternatives and the layers Docker is often confused with.

## Why this is not only "Docker"

| Layer                  | What it is                                                            | Examples                                                          |
| ---------------------- | --------------------------------------------------------------------- | ----------------------------------------------------------------- |
| **Desktop / local UX** | GUI or lightweight VM wrapper for developers                          | Docker Desktop, Podman Desktop, Rancher Desktop, OrbStack, Colima |
| **Engine / CLI suite** | Build, run, compose workflows people mean by "Docker"                 | Docker Engine + CLI, Podman                                       |
| **Runtime**            | Low-level process that runs containers (often under an engine or K8s) | containerd, CRI-O, runc                                           |
| **Image builder**      | Produce OCI images (sometimes without a daemon)                       | Dockerfile/`docker build`, Buildah, Kaniko                        |

Comparing OrbStack to containerd is a category error — this tree keeps those layers separate.

## Core ecosystems

| Tool       | Focus                                                   | Page                       |
| ---------- | ------------------------------------------------------- | -------------------------- |
| **Docker** | Engine, CLI, Compose, images, volumes, platforms        | [docker](docker/README.md) |
| **Podman** | Daemonless / rootless OCI engine; Docker-compatible CLI | [podman](podman/README.md) |

## Other layers

| Category               | Description                                   | Page                                               |
| ---------------------- | --------------------------------------------- | -------------------------------------------------- |
| **Desktop interfaces** | Local GUI / VM alternatives to Docker Desktop | [desktop-interfaces](desktop-interfaces/README.md) |
| **Runtimes**           | Production/K8s-oriented low-level engines     | [runtimes](runtimes/README.md)                     |
| **Image builders**     | Daemonless / cluster-native OCI builds        | [image-builders](image-builders/README.md)         |

## Related

- Host OS topics: [Linux](../linux/README.md)
- Package managers vs container images: [package managers](../linux/package-managers/README.md)
