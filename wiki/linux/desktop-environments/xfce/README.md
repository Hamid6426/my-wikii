# XFCE

Tags: `desktop` `desktop-environments` `gui` `linux` `xfce`

GTK-based traditional desktop: panels, menu, modest resource use. Favors stability and classic workflows over radical redesigns.

## Feel

Light, familiar, classic. Good on older hardware and for users who want a simple panel desktop.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (GTK) + Thunar, Mousepad, …       │
├─────────────────────────────────────────┤
│  xfce4-panel · xfdesktop · appfinder    │
├─────────────────────────────────────────┤
│  xfwm4 (window manager)                 │
├─────────────────────────────────────────┤
│  X11 (common) · Wayland (emerging)      │
├─────────────────────────────────────────┤
│  xfce4-session · D-Bus                  │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴──────┐
      │ LightDM │  (typical on XFCE flavors)
      └─────────┘
```

| Layer           | Component                                                       | Role                                |
| --------------- | --------------------------------------------------------------- | ----------------------------------- |
| Display manager | **[LightDM](../../display-managers/lightdm/README.md)** (often) | Login                               |
| Session         | **xfce4-session**                                               | Starts panel, desktop, WM, services |
| Shell / UI      | **xfce4-panel**, **xfdesktop**                                  | Panel/applets and desktop icons     |
| Window manager  | **xfwm4**                                                       | Windows, compositing (X11)          |
| Toolkit         | **GTK**                                                         | Apps and most XFCE UI               |
| Settings        | **xfce4-settings**                                              | Appearance, WM tweaks, displays     |
| File manager    | **Thunar**                                                      | Files / folders                     |

XFCE is modular: you can swap the WM or run panel pieces independently. X11 is still the mainstream path; Wayland work exists but is not the universal default yet.

## Common distro homes

Xubuntu, Fedora XFCE spin, Debian XFCE, Manjaro XFCE, Mint XFCE edition.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [MATE](../mate/README.md) — another traditional GTK desktop
