# xterm

Tags: `cli` `linux` `shells-and-terminals` `terminal` `terminals` `x11` `xterm`

Classic X11 terminal emulator from the X Project. Ubiquitous, battle-tested, and minimal by modern GUI standards — still useful for debugging and compatibility.

## Role

- Always-there terminal on X11 systems
- Reference behavior for many escape sequences / terminfo issues
- Resources via `~/.Xresources` / `xrdb` (traditional setup)

## Quick facts

| Item    | Detail                                 |
| ------- | -------------------------------------- |
| Package | `xterm`                                |
| Display | X11                                    |
| UX      | Sparse menus; keyboard/resource driven |

## Notes

Modern desktops default to GNOME Terminal/Konsole/etc., but `xterm` remains a reliable fallback. Set `TERM` carefully when mixing with multiplexers and remote SSH.

## Related

- [Terminals index](../README.md)
- [Display servers](../../../display-servers/README.md)
- [Shells](../../shells/README.md)
