# Shells

Tags: `cli` `linux` `shell` `shells` `shells-and-terminals`

A **shell** is the command-line interpreter: it shows a prompt, parses what you type, runs programs, and (for most shells) runs scripts.

Your login/default shell is usually set in `/etc/passwd` or via `chsh`. Check with:

```bash
echo $SHELL
ps -p $$
```

## Pages

| Shell             | Notes                                           | Page                         |
| ----------------- | ----------------------------------------------- | ---------------------------- |
| Bash              | Default on most Linux distros                   | [bash](bash/README.md)       |
| Zsh               | Powerful interactive shell; Oh My Zsh ecosystem | [zsh](zsh/README.md)         |
| Fish              | Friendly syntax, great defaults, not POSIX      | [fish](fish/README.md)       |
| Dash / POSIX `sh` | Minimal; often `/bin/sh` on Debian/Ubuntu       | [dash](dash/README.md)       |
| Nushell           | Structured data pipelines                       | [nushell](nushell/README.md) |

## Shell vs terminal

The shell runs **inside** a [terminal emulator](../terminals/README.md) (or on a virtual console `tty`). Swapping Kitty for Alacritty does not change Bash → Zsh; that is `chsh` / your terminal profile's command.

## Related

- [Shells and terminals index](../README.md)
- [Terminals](../terminals/README.md)
