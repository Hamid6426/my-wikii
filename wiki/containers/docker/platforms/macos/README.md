# Docker on macOS

Tags: `containers` `desktop` `docker` `install` `macos` `oci` `platforms` `vm`

macOS has no Linux kernel, so Docker runs Linux containers inside a **Linux virtual machine**. **Docker Desktop for Mac** manages that VM and exposes the familiar `docker` / `docker compose` CLIs.

## How it fits together

```
macOS apps / Terminal
        ↓
   Docker CLI
        ↓
 Docker Desktop VM (Linux)
        ↓
   dockerd + containers
```

You do **not** get "containers on the XNU kernel" for normal Linux images — the VM is required.

## Install

> **Licensing (verify current terms):** Docker Desktop is free for personal use, education, and many small businesses; larger commercial use may require a paid Docker subscription. Check [Docker's pricing/subscription docs](https://www.docker.com/pricing/) before deploying org-wide — terms change. Alternatives: [Colima](../../../desktop-interfaces/colima/README.md), [OrbStack](../../../desktop-interfaces/orbstack/README.md), [Podman Desktop](../../../podman/podman-desktop/README.md).

1. Install **Docker Desktop for Mac** from Docker's site (choose Apple Silicon or Intel build).
2. Launch Desktop, finish onboarding, wait until the engine is running.
3. Verify in Terminal:

```bash
docker version
docker info
docker run --rm hello-world
```

Homebrew cask is also commonly used: `brew install --cask docker` (still installs Docker Desktop).

## Apple Silicon (M1/M2/M3/…) vs Intel

| Topic              | Notes                                                                                      |
| ------------------ | ------------------------------------------------------------------------------------------ |
| **Image CPU arch** | Prefer `arm64` images; many multi-arch manifests work via buildx                           |
| **Emulation**      | `amd64` images can run via emulation (slower); watch for "exec format" / platform warnings |
| **Flags**          | `docker run --platform=linux/amd64 …` when you must force x86_64                           |
| **Build**          | `docker buildx build --platform linux/amd64,linux/arm64 …` for multi-arch                  |

```bash
uname -m
# arm64 on Apple Silicon

docker buildx ls
```

## Resources & files

Docker Desktop → Settings → Resources: CPUs, memory, swap, disk.

**Bind mounts:** macOS ↔ Linux VM filesystem sharing is better than older days but still not identical to native Linux. Prefer:

- Keep hot reload / huge `node_modules` workflows aware of sync cost
- Use named volumes for databases instead of binding DB files from macOS paths when possible

**VirtioFS** / Virtio options in Desktop improve file-sharing performance — leave recommended defaults unless docs say otherwise for your Desktop version.

## Alternatives

See [Desktop interfaces](../../../desktop-interfaces/README.md) ([Colima](../../../desktop-interfaces/colima/README.md), [OrbStack](../../../desktop-interfaces/orbstack/README.md), [Rancher Desktop](../../../desktop-interfaces/rancher-desktop/README.md), [Podman Desktop](../../../podman/podman-desktop/README.md)). This wiki's default Mac path remains Docker Desktop unless you have a reason to switch.

## Common gotchas

| Symptom                       | Likely cause                                             |
| ----------------------------- | -------------------------------------------------------- |
| Engine starting forever       | Desktop VM stuck; restart Desktop / check virtualization |
| Slow npm/file watch           | Bind mount performance; move to volume or tune watchers  |
| Wrong arch binary in image    | Build/run without multi-arch; set `--platform`           |
| Port already allocated        | Another local service or old container bound the port    |
| Credential helper / Hub login | Desktop keychain integration prompts                     |

## Related

- [Platforms index](../README.md)
- [Docker index](../../README.md)
- [Containers index](../../../README.md)
- [Desktop interfaces](../../../desktop-interfaces/README.md)
- [Docker on Linux](../linux/README.md) — native Engine (no Mac VM layer)
- [Docker on Windows](../windows/README.md) — WSL2 instead of a Mac VM
