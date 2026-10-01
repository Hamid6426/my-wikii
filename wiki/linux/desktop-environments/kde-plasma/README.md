# KDE Plasma

Tags: `desktop` `desktop-environments` `gui` `kde` `kde-plasma` `linux` `wayland`

Qt-based desktop environment. Highly configurable; KWin handles window management and compositing. Can look traditional, Windows-like, or heavily customized.

## Feel

Feature-rich: panels, widgets, activities, extensive system settings. Heavier than XFCE/LXQt if you enable everything, but tunable.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (Qt / KDE Frameworks / Flatpak)   │
├─────────────────────────────────────────┤
│  Plasma Shell (panels, widgets, …)      │
├─────────────────────────────────────────┤
│  KWin (WM + compositor)                 │
├─────────────────────────────────────────┤
│  Wayland or X11 session                 │
├─────────────────────────────────────────┤
│  systemd user session · D-Bus           │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴───┐
      │ SDDM │  (typical)
      └──────┘
```

| Layer           | Component                                                   | Role                                            |
| --------------- | ----------------------------------------------------------- | ----------------------------------------------- |
| Display manager | **[SDDM](../../display-managers/sddm/README.md)** (typical) | Login / session chooser                         |
| Session         | **plasma-workspace** / `startplasma-*`                      | Session startup                                 |
| Shell / UI      | **Plasma Shell** (`plasmashell`)                            | Panels, desktop, widgets, kickoff               |
| WM + compositor | **KWin**                                                    | Window management + compositing (Wayland & X11) |
| Toolkit / libs  | **Qt** + **KDE Frameworks (KF6)**                           | Apps and desktop libraries                      |
| Settings        | **System Settings**                                         | Central configuration                           |
| File manager    | **Dolphin**                                                 | Files / folders                                 |
| Portals         | **xdg-desktop-portal-kde**                                  | Sandboxed app integration                       |

Plasma Workspaces split cleanly: KWin owns windows; plasmashell owns the desktop chrome. Themes/widgets plug into the shell without replacing KWin.

## Common distro homes

Kubuntu, Fedora KDE Plasma Desktop, openSUSE (Plasma default on some products), Manjaro KDE, Garuda, Debian KDE.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
