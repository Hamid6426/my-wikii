# Window managers

Tags: `gui` `linux` `window-managers` `wm`

Window managers (and Wayland compositors used as a standalone session) control how windows are placed and focused. They are **not** full desktop environments — no complete suite of panels, settings apps, and default applications unless you add those yourself.

## Why this is separate from desktop environments

|              | [Desktop environment](../desktop-environments/README.md) | Window manager / minimal stack   |
| ------------ | -------------------------------------------------------- | -------------------------------- |
| **Scope**    | Full GUI session                                         | Mostly window layout + focus     |
| **Defaults** | Batteries included                                       | You assemble bar, launcher, etc. |
| **Examples** | GNOME, Plasma, XFCE                                      | i3, Sway, Hyprland, Openbox      |

## Pages

| Stack    | Type                                | Page                           |
| -------- | ----------------------------------- | ------------------------------ |
| i3       | Tiling WM (X11)                     | [i3](i3/README.md)             |
| Sway     | Tiling compositor (Wayland)         | [sway](sway/README.md)         |
| Hyprland | Dynamic tiling compositor (Wayland) | [hyprland](hyprland/README.md) |
| bspwm    | Tiling WM (X11)                     | [bspwm](bspwm/README.md)       |
| Openbox  | Stacking WM (X11)                   | [openbox](openbox/README.md)   |

## Related

- [Desktop environments](../desktop-environments/README.md) — full DEs
- [Display managers](../display-managers/README.md) — login greeters
- [Display servers](../display-servers/README.md) — X11 / Wayland
- [Distros](../distros/README.md)
- [Linux index](../README.md)
