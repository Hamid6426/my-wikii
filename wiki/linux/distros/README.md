# Distros

Tags: `distro` `distros` `linux`

A Linux distribution (distro) is the Linux kernel plus userland tools, a package manager, defaults, and often a release/support model. Distros differ mainly in packaging, update cadence, defaults, and target audience — not in "being Linux."

## Quick pick

| Goal                                   | Good starting points                         |
| -------------------------------------- | -------------------------------------------- |
| Desktop, easy, lots of guides          | Ubuntu, Linux Mint, Fedora Workstation       |
| Stable servers / long support          | Debian, Ubuntu LTS, Rocky Linux, AlmaLinux   |
| Bleeding-edge packages                 | Arch, openSUSE Tumbleweed, Fedora            |
| Immutable / container-friendly desktop | Fedora Silverblue, openSUSE Aeon, Vanilla OS |
| Lightweight / older hardware           | MX Linux, Lubuntu, Alpine (servers), Void    |
| Security-focused                       | Qubes OS, Tails (live), Kicksecure           |
| Embedded / appliances                  | Alpine, OpenWrt, Yocto-based builds          |

## Comparison (qualitative)

Rough feel for **defaults** — not benchmarks. Same kernel can feel different under a heavy DE, snaps/Flatpaks, or a minimal WM. Pick for workflow and docs you trust, not a single “winner” column.

| Distro / family                                     | Footprint / “performance” feel      | Default GUI (typical)     | UX / polish                                  | Updates / stability                        | Docs & ecosystem                                       |
| --------------------------------------------------- | ----------------------------------- | ------------------------- | -------------------------------------------- | ------------------------------------------ | ------------------------------------------------------ |
| [Ubuntu](ubuntu/README.md)                          | Medium; snaps can add weight        | GNOME (flavors vary)      | Smooth onboarding; lots of guided help       | Fixed + LTS; predictable                   | Huge guides, forums, cloud images                      |
| [Debian](ubuntu/debian/README.md)                   | Medium–light if you stay minimal    | Installer choice / none   | Conservative defaults; less “hand-holding”   | Very stable (esp. stable); slower new apps | Strong; slightly less desktop hand-holding than Ubuntu |
| Linux Mint _(Ubuntu/Debian-based)_                  | Medium; lighter than full GNOME     | Cinnamon / MATE / XFCE    | Familiar, Windows-friendly desktop           | Tracks Ubuntu LTS bases                    | Easy multimedia; great for non-power users             |
| [Fedora](fedora/README.md)                          | Medium; modern stack, Flatpak-heavy | GNOME or Plasma spins     | Polished Wayland desktop; SELinux on         | ~6‑month releases; fairly fresh            | Good docs; strong for developers                       |
| [RHEL](fedora/rhel/README.md) / Rocky / Alma        | Medium; tuned for servers           | Optional / minimal common | Enterprise tooling over consumer polish      | Long support; conservative                 | Vendor + clone docs; certs/training                    |
| [Arch](arch/README.md)                              | Light base; you choose the weight   | None (you install)        | DIY; excellent once set up                   | Rolling; you own breakage                  | Arch Wiki is top-tier                                  |
| EndeavourOS / Manjaro _(Arch-based)_                | Medium (preconfigured desktop)      | Various (KDE/GNOME/XFCE…) | Installer + defaults; less pure DIY          | Near-rolling / delayed (Manjaro)           | Arch Wiki + derivative forums                          |
| [openSUSE](others/opensuse/README.md)               | Medium                              | Plasma / GNOME common     | YaST, solid admin UX                         | Leap (stable) or Tumbleweed (rolling)      | Good manuals; smaller desktop mindshare                |
| [Alpine](others/alpine/README.md)                   | Very light (musl, busybox-ish)      | Rarely a full desktop     | Server/container/embed focus                 | Stable + edge; not a desktop-first UX      | Strong for containers/images                           |
| [NixOS](others/nixos/README.md)                     | Medium; declarative cost elsewhere  | You declare it            | Unique; steep; reproducible                  | Channel/flake model; rollbacks shine       | Excellent for power users; niche learning curve        |
| [Gentoo](others/gentoo/README.md)                   | Can be very tuned (compile cost)    | You build it              | Maximum control; slow to start               | Source-based; you set the pace             | Handbook is deep                                       |
| [Void](others/void/README.md)                       | Light                               | Optional / community      | Minimalist; runit instead of systemd         | Rolling                                    | Smaller community                                      |
| [Specialty / immutable](others/specialty/README.md) | Medium; image-based OS              | GNOME/Plasma common       | App-centric (Flatpak); host is read-only-ish | Atomic updates / rebases                   | Growing; different mental model                        |

**Reading the columns**

- **Footprint / performance:** Idle RAM and “snappiness” of a default install more than raw kernel speed. A light WM on Ubuntu can beat a full Plasma Arch box.
- **Default GUI:** What you get if you take the main desktop image — not what you _can_ install later. See [desktop environments](../desktop-environments/README.md) and [window managers](../window-managers/README.md).
- **UX / polish:** Installer, first-boot, settings apps, and how much the distro steers you. Not “beauty.”
- **Updates / stability:** Cadence and how often you must babysit upgrades — not security quality (all of these can be secured well).

## Distro families

Most popular distros descend from a few roots. Knowing the family tells you the package format and many admin habits.

```
Debian ──► Ubuntu ──► Mint, Pop!_OS, elementary, Zorin, …
          └─► Kali, Parrot, …

Fedora ──► RHEL ──► Rocky, AlmaLinux, CentOS Stream, Oracle Linux, …

Arch ──► Manjaro, EndeavourOS, Garuda, …

SUSE ──► openSUSE Leap / Tumbleweed, SLES

Independent: Gentoo, Void, Alpine, NixOS, Slackware, Solus, …
```

## Package managers (cheat sheet)

| Family          | Format         | Install / update (typical)               |
| --------------- | -------------- | ---------------------------------------- |
| Debian / Ubuntu | `.deb`         | `apt update && apt install <pkg>`        |
| Fedora / RHEL   | `.rpm`         | `dnf install <pkg>`                      |
| Arch            | packages / AUR | `pacman -S <pkg>`; AUR via helper        |
| openSUSE        | `.rpm`         | `zypper install <pkg>`                   |
| Alpine          | `.apk`         | `apk add <pkg>`                          |
| Void            | XBPS           | `xbps-install -S <pkg>`                  |
| NixOS           | Nix            | `nix-env` / flakes / `configuration.nix` |
| Gentoo          | ebuilds        | `emerge <pkg>`                           |

## Release models

- **Fixed release (point release):** Snapshots every N months (e.g. Ubuntu 24.04, Fedora 41). Predictable, easy to document.
- **LTS:** Longer support windows (Ubuntu LTS ~5 years; RHEL much longer with subscriptions). Prefer for production.
- **Rolling:** Continuous updates (Arch, Tumbleweed, Void). Always current; you own breakage risk and backup discipline.
- **Semi-rolling / hybrid:** e.g. Debian testing, or "stable base + flatter apps" (Flatpak/Snap/AppImage).

## Desktop vs server

- **Desktop:** Ships a GUI stack (GNOME, KDE Plasma, XFCE, etc.), firmware helpers, multimedia codecs policies vary by distro.
- **Server:** Minimal install, SSH-first, focused on stability and long support. Same package family as the desktop sibling is common (Debian/Ubuntu, RHEL clones, etc.).
- **Cloud images:** Many distros publish optimized cloud/VM images (cloud-init, small footprint).

## Pages in this section

- [Ubuntu](ubuntu/README.md) ([Debian](ubuntu/debian/README.md))
- [Fedora](fedora/README.md) ([RHEL](fedora/rhel/README.md))
- [Arch](arch/README.md)
- [Others](others/README.md) — openSUSE, Alpine, NixOS, Gentoo, Void, Slackware, specialty
- Cheat sheet: [Package managers](../package-managers/README.md)
- Containers: [Containers](../../containers/README.md) · [Docker](../../containers/docker/README.md) · [Podman](../../containers/podman/README.md)

## Common gotchas

- **Codecs & proprietary drivers:** Ubuntu/Mint are often easier out of the box; Fedora/Debian may need extra repos or Flatpaks.
- **Immutable systems:** Updates are atomic OS images; don't treat `/usr` like a classic mutable distro.
- **Snaps / Flatpak / AppImage:** Cross-distro apps; great for isolation, but duplicates runtimes and can feel "non-native."
- **Init system:** Almost everything mainstream uses **systemd**; exceptions include Alpine (OpenRC), Void (runit), some Gentoo setups.
- **"Best distro":** Depends on docs you trust, hardware, and whether you want stability or newest packages — not marketing.

## Further reading

- DistroWatch — release news and popularity (use critically)
- Each project's official docs (always prefer over random blog posts for install/upgrade)
- `uname -a`, `/etc/os-release` — identify what you're actually running
