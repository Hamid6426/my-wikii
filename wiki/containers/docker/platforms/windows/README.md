# Docker on Windows

Tags: `containers` `desktop` `docker` `install` `oci` `platforms` `windows` `wsl`

On Windows you usually run **Linux containers** through a lightweight Linux environment, or (less often) **Windows containers** on Windows Server / matching Windows client builds.

## Two container types

| Type                   | What runs inside                                    | Needs                                                                |
| ---------------------- | --------------------------------------------------- | -------------------------------------------------------------------- |
| **Linux containers**   | Linux userland on a Linux kernel                    | WSL2 backend (recommended) or Hyper-V VM                             |
| **Windows containers** | Windows base images (`mcr.microsoft.com/windows/…`) | Windows host compatible with the image; Hyper-V or process isolation |

Most app development (Node, Python, Go, typical Docker Hub images) uses **Linux containers**.

## Docker Desktop for Windows

> **Licensing (verify current terms):** Docker Desktop is free for personal use, education, and many small businesses; larger commercial use may require a paid Docker subscription. Check [Docker's pricing/subscription docs](https://www.docker.com/pricing/) before deploying org-wide — terms change.

The mainstream developer setup:

1. Install **Docker Desktop**.
2. Enable **WSL 2** backend (Settings → General).
3. Install a WSL distro (Ubuntu is common): `wsl --install`.
4. Confirm:

```powershell
docker version
docker info
wsl -l -v
```

Docker Desktop starts the Engine inside a WSL2 distro/utility VM and exposes the `docker` CLI on Windows (PowerShell, cmd) and inside WSL.

### WSL2 tips

- Keep project files in the **WSL filesystem** (`\\wsl$\…` / `~/…`) for much better bind-mount performance than `C:\…` mounts into Linux containers.
- From WSL: `docker` often talks to Desktop's engine automatically when Desktop integration is enabled for that distro.
- Update WSL: `wsl --update`.

### Resources

In Docker Desktop → Settings → Resources: CPUs, memory, disk image size. Starving memory causes random build/run failures.

## Windows containers (overview)

Switch Docker Desktop to **Windows containers** (tray icon / context menu) when you need Windows base images (IIS, .NET Framework full, etc.).

Caveats:

- Images are large; tags must match host Windows version more carefully than Linux images.
- Not interchangeable with Linux containers in one swarm of the same default setup — you pick a mode.
- Many Hub tutorials assume Linux; read the image OS before pulling.

```powershell
# example idea only — exact tags depend on host build

docker pull mcr.microsoft.com/windows/nanoserver:ltsc2022
```

## Install without Desktop?

Possible with Engine-ish setups or upstream scripts, but **Docker Desktop + WSL2** is what most Windows developers should document and support first. CI on Windows often uses cloud runners with preinstalled Docker or builds Linux images via remote Linux agents.

## Common gotchas

| Symptom                       | Likely cause                                                 |
| ----------------------------- | ------------------------------------------------------------ |
| `docker` not found            | Desktop not running / CLI not on PATH                        |
| Slow binds / watchers         | Project on `/mnt/c` instead of WSL home                      |
| VPN breaks pulls              | Desktop networking / DNS conflict                            |
| Hyper-V / WSL errors          | Virtualization disabled in BIOS, or Windows features missing |
| Permission weirdness in binds | UID/GID vs Windows file ownership on shared mounts           |

## Related

- [Platforms index](../README.md)
- [Docker index](../../README.md)
- [Containers index](../../../README.md)
- [Desktop interfaces](../../../desktop-interfaces/README.md)
- [Docker on Linux](../linux/README.md) — same Engine model once you're inside WSL
- [Docker on macOS](../macos/README.md)
