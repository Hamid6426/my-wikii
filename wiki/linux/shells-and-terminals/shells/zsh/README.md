# Zsh

Tags: `cli` `linux` `shell` `shells` `shells-and-terminals` `zsh`

**Zsh** is a powerful interactive shell with strong completion, globbing, and customization. Popular as a Bash alternative for daily CLI use; scripting is capable but Bash remains more universal for portable scripts.

## Role

- Interactive shell of choice for many developers
- Large plugin/theme ecosystems (Oh My Zsh, Prezto, Antidote, etc.)
- Config: `~/.zshrc` (main), `~/.zprofile` / `~/.zshenv` for env/login nuances

## Quick facts

| Item          | Detail                                                              |
| ------------- | ------------------------------------------------------------------- |
| Package       | `zsh`                                                               |
| Binary        | `/usr/bin/zsh` or `/bin/zsh`                                        |
| Completion    | Excellent built-in completion system                                |
| Compatibility | Can emulate sh/bash to a degree; not a drop-in for all Bash scripts |

## Useful bits

```bash
zsh --version
chsh -s $(which zsh)
# first run may walk you through a short newuser wizard

```

Features people switch for: smarter tab-completion, path expansion, themes/prompts (often via frameworks), shared history options.

## Notes

Frameworks are optional — plain Zsh + a few lines in `~/.zshrc` is enough. Keep heavy plugin stacks lean if startup time matters.

## Related

- [Shells index](../README.md)
- [Bash](../bash/README.md)
- [Fish](../fish/README.md)
- [Terminals](../../terminals/README.md)
