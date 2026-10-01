# Docker on Linux

Tags: `containers` `docker` `engine` `install` `linux` `oci` `platforms`

On Linux, **Docker Engine** runs natively: containers share the host kernel. No Linux VM is required for Linux containers (unlike macOS/Windows Desktop).

## What you install

| Piece                     | Role                                                              |
| ------------------------- | ----------------------------------------------------------------- |
| **dockerd** (Engine)      | Daemon that builds/runs containers                                |
| **docker** CLI            | Client talking to the daemon (Unix socket)                        |
| **containerd** / **runc** | Lower-level runtime stack (pulled in by Docker packages)          |
| **Docker Compose** plugin | `docker compose` (v2 plugin; prefer over legacy `docker-compose`) |

Optional: **Docker Desktop for Linux** exists, but most servers and many desktops use Engine + CLI only.

## Install (high level)

Prefer each distro's **official Docker docs** (download.docker.com) over random scripts when you care about updates and packaging.

Typical flow:

1. Remove old distro `docker.io` / `podman-docker` packages if conflicting (optional, depends on distro).
2. Add Docker's apt/yum/dnf repo for your distro.
3. Install `docker-ce`, `docker-ce-cli`, `containerd.io`, `docker-buildx-plugin`, `docker-compose-plugin`.
4. Start and enable the service:

```bash
sudo systemctl enable --now docker
sudo docker run --rm hello-world
```

### Distro notes

| Family             | Notes                                                                                                     |
| ------------------ | --------------------------------------------------------------------------------------------------------- |
| Debian / Ubuntu    | Official Docker CE packages are common for latest Engine; Ubuntu also ships `docker.io` (version may lag) |
| Fedora / RHEL-like | Docker CE repo or use **Podman** (daemonless) as an alternative ecosystem                                 |
| Arch               | `docker` / `docker-compose` in official/community repos; enable `docker.service`                          |

Always confirm against current Docker + distro documentation — package names shift.

## Permission model

By default the CLI talks to `unix:///var/run/docker.sock` as root. Common approaches:

| Approach                   | Trade-off                                                                                  |
| -------------------------- | ------------------------------------------------------------------------------------------ |
| `sudo docker …`            | Simple; every command needs sudo                                                           |
| Add user to `docker` group | Convenient; **group membership is effectively root-equivalent** via the socket             |
| **Rootless Docker**        | Daemon in user namespace; better isolation, some features limited (ports, storage drivers) |

```bash
# after adding yourself to docker group, re-login

groups
docker ps
```

## Storage & networking (Linux-specific feel)

- **Storage drivers:** `overlay2` is the usual default on modern kernels.
- **Bridge network:** containers get virtual eth interfaces; publish ports with `-p host:container`.
- **Firewall:** `iptables`/`nftables` rules are manipulated by Docker for published ports — surprising interactions with UFW/firewalld are a common gotcha.

```bash
docker info | less
# look for: Server Version, Storage Driver, Cgroup Driver, Runtimes

```

## cgroups & systemd

Modern distros use **cgroup v2**. Docker integrates with systemd (`cgroup driver: systemd` is typical). If containers fail to start after big distro upgrades, check cgroup mode and Engine compatibility notes.

## Desktop vs server

| Use                | Typical setup                                                                                                                   |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------- |
| Server / CI runner | Engine + Compose plugin; no GUI                                                                                                 |
| Desktop            | Engine, or Docker Desktop for Linux; integrate with your [terminal](../../../../linux/shells-and-terminals/terminals/README.md) |

## Related

- [Platforms index](../README.md)
- [Docker index](../../README.md)
- [Containers index](../../../README.md)
- [Podman](../../../podman/README.md)
- [Docker on Windows](../windows/README.md) — WSL2 path still ends in a Linux Engine
- [Docker on macOS](../macos/README.md)
- [Linux kernel](../../../../linux/linux-kernel/README.md)
- [Package managers](../../../../linux/package-managers/README.md)
- [Ubuntu](../../../../linux/distros/ubuntu/README.md) · [Fedora](../../../../linux/distros/fedora/README.md)
