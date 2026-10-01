# GNOME

Tags: `desktop` `desktop-environments` `gnome` `gui` `linux` `wayland`

GTK-based desktop environment. Opinionated design centered on the Activities overview. Default on Fedora Workstation and Ubuntu Desktop.

## Feel

Modern, fewer knobs out of the box. Extensions (via Extension Manager / browser integration) add many missing toggles.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (GTK / Flatpak) + Extensions      │
├─────────────────────────────────────────┤
│  GNOME Shell (UI: overview, panel, …)   │
├─────────────────────────────────────────┤
│  Mutter (WM + compositor)               │
├─────────────────────────────────────────┤
│  Wayland (default) / X11 session        │
├─────────────────────────────────────────┤
│  systemd user session · D-Bus           │
└─────────────────────────────────────────┘
         ▲
         │ display manager
      ┌──┴──┐
      │ GDM │
      └─────┘
```

| Layer           | Component                                                              | Role                                         |
| --------------- | ---------------------------------------------------------------------- | -------------------------------------------- |
| Display manager | **[GDM](../../display-managers/gdm/README.md)**                        | Login / session chooser                      |
| Session         | `gnome-session`                                                        | Starts Shell and session services            |
| Shell / UI      | **GNOME Shell**                                                        | Panel, Activities, notifications, app grid   |
| WM + compositor | **Mutter**                                                             | Windows, input, effects (Wayland compositor) |
| Toolkit         | **GTK 4** / libadwaita (apps); Shell uses its own JS/C stack on Mutter | App look and widgets                         |
| Settings        | **GNOME Settings** + GSettings / dconf                                 | System and desktop prefs                     |
| File manager    | **Nautilus** (Files)                                                   | Files / folders                              |
| Portals         | **xdg-desktop-portal-gnome**                                           | Sandboxed app access (Flatpak, etc.)         |

Wayland is the default path on current GNOME; X11 sessions are legacy/fallback on many distros.

## Common distro homes

Ubuntu Desktop, Fedora Workstation, Debian GNOME, Pop!_OS (historically GNOME; moving toward COSMIC).

## Related

- [Desktop environments index](../README.md)
- [GDM](../../display-managers/gdm/README.md)
- [Display servers](../../display-servers/README.md)
- [COSMIC](../cosmic/README.md) — System76's successor direction for Pop!_OS
