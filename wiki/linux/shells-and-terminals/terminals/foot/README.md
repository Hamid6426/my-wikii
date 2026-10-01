# foot

Tags: `cli` `foot` `linux` `shells-and-terminals` `terminal` `terminals` `wayland`

Lightweight, fast **Wayland-native** terminal emulator. Popular on Sway, Hyprland, and other wlroots-style setups.

## Role

- Default-ish choice on many minimal Wayland sessions
- Low latency; simple config
- Config: `~/.config/foot/foot.ini`

## Quick facts

| Item        | Detail                                              |
| ----------- | --------------------------------------------------- |
| Package     | `foot`                                              |
| Display     | Wayland (not an X11 app)                            |
| Server mode | `foot --server` / `footclient` for fast new windows |

## Notes

If you still need X11-only sessions, use something else ([Alacritty](../alacritty/README.md), [Kitty](../kitty/README.md), xterm). On pure Wayland tiling WMs, foot is a common default.

## Related

- [Terminals index](../README.md)
- [Display servers](../../../display-servers/README.md)
- [Sway](../../../window-managers/sway/README.md)
- [Hyprland](../../../window-managers/hyprland/README.md)
- [Shells](../../shells/README.md)
