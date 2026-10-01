# Display managers

Tags: `display-managers` `dm` `gui` `linux`

A display manager (DM / login manager) shows the greeter, authenticates the user, and starts a chosen desktop or window-manager session. It sits **before** the DE/WM — not the same as the [display server](../display-servers/README.md) (X11 / Wayland).

## Why this is separate

|                  | Display manager            | [Desktop environment](../desktop-environments/README.md) / [WM](../window-managers/README.md) |
| ---------------- | -------------------------- | --------------------------------------------------------------------------------------------- |
| **When it runs** | Login / session select     | After login                                                                                   |
| **Job**          | Auth + start session       | Desktop UI / window layout                                                                    |
| **Examples**     | GDM, SDDM, LightDM, greetd | GNOME, Plasma, i3, Sway                                                                       |

## Pages

| DM      | Typical home                       | Page                         |
| ------- | ---------------------------------- | ---------------------------- |
| GDM     | GNOME                              | [gdm](gdm/README.md)         |
| SDDM    | KDE Plasma (often LXQt too)        | [sddm](sddm/README.md)       |
| LightDM | XFCE, Cinnamon, MATE, many flavors | [lightdm](lightdm/README.md) |
| greetd  | Minimal / Wayland WM stacks        | [greetd](greetd/README.md)   |

DE-specific greeters (e.g. **cosmic-greeter**) usually plug into or replace the distro DM — see that DE’s page.

## Common ops

```bash
# Which DM unit is enabled (systemd)
systemctl status display-manager.service

# Switch DM (exact unit names vary by distro package)
sudo systemctl disable --now gdm.service    # example
sudo systemctl enable --now sddm.service
sudo systemctl set-default graphical.target
```

Session entries live as `.desktop` files under `/usr/share/xsessions/` and `/usr/share/wayland-sessions/` — that is what the greeter lists.

## Related

- [Desktop environments](../desktop-environments/README.md)
- [Window managers](../window-managers/README.md)
- [Display servers](../display-servers/README.md) — X11 / Wayland
- [Systemd](../systemd/README.md)
- [Linux index](../README.md)
