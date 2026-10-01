# Desktop environments

Tags: `desktop` `desktop-environments` `gui` `linux`

A desktop environment (DE) is the GUI stack on top of Linux: window management, panels, settings apps, file manager, and session services. Distros pick a default DE; most let you install others side by side.

For tiling and minimal setups that are **only** a window manager/compositor (i3, Sway, Hyprland, …), see [Window managers](../window-managers/README.md) — those are intentionally kept separate so they are not confused with full DEs.

## Concepts

| Layer                           | Role                                                   | Examples                                              |
| ------------------------------- | ------------------------------------------------------ | ----------------------------------------------------- |
| **Display manager**             | Login greeter; starts the session                      | See [display managers](../display-managers/README.md) |
| **Desktop environment**         | Full session: panels, settings, apps, integration      | GNOME, KDE Plasma, XFCE                               |
| **Window manager / compositor** | Places windows (often used alone as a minimal session) | See [window managers](../window-managers/README.md)   |

See also: [Display servers](../display-servers/README.md) (X11 / Wayland).

## Pages

| DE           | Page                               |
| ------------ | ---------------------------------- |
| GNOME        | [gnome](gnome/README.md)           |
| KDE Plasma   | [kde-plasma](kde-plasma/README.md) |
| XFCE         | [xfce](xfce/README.md)             |
| Cinnamon     | [cinnamon](cinnamon/README.md)     |
| MATE         | [mate](mate/README.md)             |
| LXQt         | [lxqt](lxqt/README.md)             |
| Budgie       | [budgie](budgie/README.md)         |
| COSMIC       | [cosmic](cosmic/README.md)         |
| Pantheon     | [pantheon](pantheon/README.md)     |
| Deepin (DDE) | [deepin](deepin/README.md)         |

## How distros ship desktops

- **Official edition / flavor:** Ubuntu Desktop (GNOME), Kubuntu (Plasma), Xubuntu (XFCE), Fedora Workstation vs KDE spin, etc.
- **Spin / community ISO:** Same repos, different default session packages.
- **Install later:** install the DE packages, then pick the session at the [display manager](../display-managers/README.md) (GDM, SDDM, LightDM, …).

## Related

- [Display managers](../display-managers/README.md) — login greeters
- [Window managers](../window-managers/README.md) — tiling / minimal stacks (not DEs)
- [Display servers](../display-servers/README.md)
- Distro defaults: [Distros](../distros/README.md)
- GPU / firmware: [Linux kernel](../linux-kernel/README.md)
- [Linux index](../README.md)
