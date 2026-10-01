# tmux

Tags: `cli` `linux` `multiplexer` `multiplexers` `shells-and-terminals` `tmux`

Classic terminal multiplexer: sessions, windows, panes. Config: `~/.tmux.conf`.

## Basics

```bash
tmux
tmux new -s work
tmux ls
tmux attach -t work
```

Default prefix: `Ctrl+b`, then:

| Key            | Action                           |
| -------------- | -------------------------------- |
| `c`            | New window                       |
| `%` / `"`      | Split vertical / horizontal      |
| `d`            | Detach                           |
| `←/→` or arrow | Move between panes (with prefix) |

## Why use it

- Survive SSH drops (`attach` later)
- Panes without depending on Kitty/WezTerm splits
- Scriptable session layouts

## Related

- [Multiplexers index](../README.md)
- [Zellij](../zellij/README.md)
- [Shells](../../shells/README.md)
