# Alacritty

Tags: `alacritty` `cli` `gpu` `linux` `shells-and-terminals` `terminal` `terminals`

GPU-accelerated terminal that stays intentionally **minimal**: fast rendering, simple YAML config, no built-in tabs/splits (use tmux/Zellij or your WM).

## Role

- Speed-focused terminal for people who multiplex elsewhere
- Cross-platform; common on Linux tiling setups
- Config: `~/.config/alacritty/alacritty.toml` (newer) or `.yml` (older)

## Quick facts

| Item        | Detail                   |
| ----------- | ------------------------ |
| Package     | `alacritty`              |
| Rendering   | GPU                      |
| Tabs/splits | Not built-in — by design |

## Notes

If you want panes inside the terminal app, prefer [WezTerm](../wezterm/README.md), [Kitty](../kitty/README.md), or a multiplexer. Alacritty shines when the WM or tmux owns layout.

## Related

- [Terminals index](../README.md)
- [Kitty](../kitty/README.md)
- [Shells](../../shells/README.md)
- [Window managers](../../../window-managers/README.md)
