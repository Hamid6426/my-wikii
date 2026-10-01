# Fedora

Tags: `distro` `distros` `dnf` `fedora` `linux` `rpm`

`.rpm` packages, primarily **DNF** today. Upstream innovation (Fedora) feeds enterprise longevity — see [RHEL](rhel/README.md) for the enterprise side and rebuilds.

## Overview

- **Role:** Upstream community distro sponsored by Red Hat; short release cycle (~6 months), ~13 months support per release.
- **Editions:** Workstation (GNOME), KDE Plasma, Server, IoT, CoreOS, **Silverblue** / **Kinoite** (atomic desktops), and spins.
- **Strengths:** New kernels/tooling early; SELinux on by default; Flatpak-friendly; strong Wayland push historically.
- **Trade-offs:** More frequent upgrades; some proprietary bits need RPM Fusion or Flatpaks.
- **Typical use:** Developers, desktop users who want newer stacks, testing ground for RHEL features.

```bash
sudo dnf upgrade
sudo dnf install <package>
dnf search <keyword>
```

## When to choose this family

- Desktop: **Fedora Workstation** if you want modern GNOME/Plasma and fresh packages.
- Production servers needing RHEL ABI/lifecycle without Red Hat subscription: **Rocky** / **Alma** — see [RHEL](rhel/README.md).
- Atomic/immutable desktop experiments: **Silverblue** / **Kinoite**.

## Related

- [RHEL](rhel/README.md) — enterprise Linux, CentOS Stream, Rocky, AlmaLinux
- [Package managers](../../package-managers/README.md) — dnf
- [Podman](../../../containers/podman/README.md) — common on Fedora
- [Docker on Linux](../../../containers/docker/platforms/linux/README.md)
- [Specialty / immutable](../others/specialty/README.md) — Silverblue
- [Distros index](../README.md)
