# Shells and terminals

Tags: `cli` `linux` `shell` `shells-and-terminals` `terminal`

Two different layers people often mix up:

| Piece                 | What it is                                         | Examples                                  |
| --------------------- | -------------------------------------------------- | ----------------------------------------- |
| **Terminal emulator** | App that draws a text console and gives you a PTY  | GNOME Terminal, Konsole, Kitty, Alacritty |
| **Shell**             | Program that reads your commands and runs programs | Bash, Zsh, Fish                           |

```
You ↔ Terminal emulator ↔ PTY ↔ Shell ↔ commands / scripts
```

- Change **fonts, tabs, GPU rendering, scrollback** → terminal
- Change **prompt, aliases, completion, scripting language** → shell

Not the same as a **desktop shell** (GNOME Shell, Plasma Shell) — that is GUI chrome, documented under [desktop environments](../desktop-environments/README.md).

## Sections

| Section                                | Description                                      |
| -------------------------------------- | ------------------------------------------------ |
| [Shells](shells/README.md)             | CLI shells: Bash, Zsh, Fish, …                   |
| [Terminals](terminals/README.md)       | Terminal emulators: Kitty, Alacritty, Konsole, … |
| [Multiplexers](multiplexers/README.md) | tmux, Zellij — sessions/panes inside a terminal  |
