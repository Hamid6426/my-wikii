# greetd

Tags: `display-managers` `dm` `greetd` `gui` `linux` `wayland`

Minimal login daemon: authenticates, then runs a configured command (often a Wayland compositor). Greeter UI is separate — **agreety**, **tuigreet**, **gtkgreet**, **wlgreet**, etc.

## Feel

Small footprint; popular with [Sway](../../window-managers/sway/README.md), [Hyprland](../../window-managers/hyprland/README.md), and other DIY Wayland stacks. You wire greeter + session yourself instead of inheriting a full DE greeter.

## Stack

- **Typical use:** standalone [window managers](../../window-managers/README.md) / compositors
- **Config:** `/etc/greetd/config.toml`
- **systemd unit:** often `greetd.service`

## Tips

- Distro packages may ship a sample config that launches agreety or tuigreet into your compositor — start there.
- Not required for WMs: many people use [GDM](../gdm/README.md) / [SDDM](../sddm/README.md) / [LightDM](../lightdm/README.md) and pick the WM session from the list, or skip a DM and use `startx` / seatd + manual compositor start.

## Related

- [Display managers index](../README.md)
- [Window managers](../../window-managers/README.md)
- [Sway](../../window-managers/sway/README.md)
- [Hyprland](../../window-managers/hyprland/README.md)
- [Display servers](../../display-servers/README.md)
