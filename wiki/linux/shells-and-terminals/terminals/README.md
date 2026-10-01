# Terminals

Tags: `cli` `linux` `shells-and-terminals` `terminal` `terminals`

A **terminal emulator** is a GUI (or TUI) app that emulates an old text terminal: it creates a PTY, draws glyphs, handles scrollback, tabs/splits, and usually launches your [shell](../shells/README.md).

## Pages

| Terminal       | Notes                             | Page                                       |
| -------------- | --------------------------------- | ------------------------------------------ |
| GNOME Terminal | Default on many GNOME desktops    | [gnome-terminal](gnome-terminal/README.md) |
| Konsole        | KDE's terminal                    | [konsole](konsole/README.md)               |
| Kitty          | GPU-accelerated; keyboard-centric | [kitty](kitty/README.md)                   |
| Alacritty      | GPU-accelerated; minimal          | [alacritty](alacritty/README.md)           |
| WezTerm        | GPU; panes, Lua config            | [wezterm](wezterm/README.md)               |
| foot           | Fast Wayland-native terminal      | [foot](foot/README.md)                     |
| xterm          | Classic X11 terminal              | [xterm](xterm/README.md)                   |

## What terminals own vs shells

| Concern                     | Usually the terminal | Usually the shell |
| --------------------------- | -------------------- | ----------------- |
| Font, colors, opacity       | ✓                    |                   |
| Tabs / splits               | ✓ (or multiplexer)   |                   |
| Scrollback                  | ✓                    |                   |
| Prompt, aliases, completion |                      | ✓                 |
| Scripts (`*.sh`)            |                      | ✓                 |

**Terminal multiplexers** (tmux, Zellij) sit between terminal and shell for persistent sessions/splits — see [Multiplexers](../multiplexers/README.md).

## Related

- [Shells and terminals index](../README.md)
- [Shells](../shells/README.md)
- [Display servers](../../display-servers/README.md)
