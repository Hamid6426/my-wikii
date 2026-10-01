# Ubuntu

Tags: `apt` `deb` `distro` `distros` `linux` `ubuntu`

`.deb` packages, `apt`/`dpkg`, and a huge shared ecosystem of docs and packages.

**Nav note:** [Debian](debian/README.md) lives under this folder for wiki browsing, but historically and technically **Debian is the upstream** — Ubuntu is a Debian-based derivative, not the other way around.

## Ubuntu

- **Role:** Popular Debian-based distro with fixed releases and strong desktop/cloud presence.
- **Cadence:** Interim releases (~9 months support); **LTS** every 2 years (~5 years standard support).
- **Strengths:** Hardware enablement, cloud images, massive tutorial surface area.
- **Trade-offs:** Snap defaults on some desktop apps; opinions differ on packaging choices.
- **Flavors:** Ubuntu Desktop, Server, plus community flavors (Kubuntu, Xubuntu, Lubuntu, Ubuntu MATE, Budgie, Unity).

```bash
lsb_release -a
sudo apt update && sudo apt upgrade
```

## Notable Ubuntu/Debian derivatives

| Distro              | Notes                                                                             |
| ------------------- | --------------------------------------------------------------------------------- |
| **Linux Mint**      | Cinnamon/MATE/XFCE; familiar desktop; easy multimedia                             |
| **Pop!\_OS**        | System76; tiling-friendly Cosmic (newer) / GNOME-based history; NVIDIA ISO option |
| **elementary OS**   | Opinionated Pantheon desktop                                                      |
| **Zorin OS**        | Windows-oriented onboarding                                                       |
| **Kali Linux**      | Security tooling (not a daily-driver recommendation by default)                   |
| **Raspberry Pi OS** | Debian-based for Raspberry Pi hardware                                            |

## When to choose this family

- You want **apt** and the largest pile of copy-paste server guides.
- You need **LTS** timelines and predictable upgrades.
- You're building on Debian packaging (CI images, containers: `debian:bookworm`, `ubuntu:24.04`).

## Related

- [Debian](debian/README.md) — upstream base for Ubuntu and many derivatives
- [Package managers](../../package-managers/README.md) — apt
- [Docker on Linux](../../../containers/docker/platforms/linux/README.md) — Engine install (Docker CE vs `docker.io`)
- [Distros index](../README.md)
