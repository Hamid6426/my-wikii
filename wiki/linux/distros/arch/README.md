# Arch

Tags: `arch` `distro` `distros` `linux` `pacman` `rolling`

Rolling-release model, **pacman**, and the **Arch Wiki** — often cited as one of the best Linux documentation resources regardless of which distro you run.

## Arch Linux

- **Role:** DIY rolling distro; you assemble the system (or use `archinstall`).
- **Strengths:** Minimal base, excellent wiki, packages stay current, KISS philosophy.
- **Trade-offs:** You maintain upgrades; breakages are possible; not ideal as a "set and forget" server without discipline.
- **AUR (Arch User Repository):** Community PKGBUILDs — powerful, but trust and review matter.

```bash
sudo pacman -Syu                 # full system upgrade
sudo pacman -S <package>         # install
sudo pacman -Ss <keyword>        # search
sudo pacman -Rns <package>       # remove + unused deps
```

Identify: `cat /etc/os-release` → `ID=arch`.

## Popular Arch-based distros

| Distro                 | Notes                                                      |
| ---------------------- | ---------------------------------------------------------- |
| **EndeavourOS**        | Arch with friendlier installer and community defaults      |
| **Manjaro**            | Calmer update cadence / own repos; diverges from pure Arch |
| **Garuda**             | Opinionated, performance-oriented desktop spins            |
| **CachyOS**            | Performance-tuned kernels/packages focus                   |
| **Archcraft / others** | Niche WM/desktop preconfigs                                |

Rule of thumb: if a guide says "Arch," verify whether you're on **pure Arch** or a derivative with different repos/tools.

## When to choose this family

- You want newest packages and are willing to read the wiki and changelogs.
- Learning how a Linux system is put together (install, bootloader, networking).
- You already live in the Arch Wiki for troubleshooting other distros.

## Related

- [Package managers](../../package-managers/README.md) — pacman
- [Docker on Linux](../../../containers/docker/platforms/linux/README.md)
- [Distros index](../README.md)
