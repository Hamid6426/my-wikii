# Fish

Tags: `cli` `fish` `linux` `shell` `shells` `shells-and-terminals`

**Fish** (Friendly Interactive SHell) focuses on good interactive defaults: suggestions, highlighting, and completions without much config. It is **not** POSIX-compatible — Fish scripts are a different language.

## Role

- Pleasant daily interactive shell
- Weaker fit when you need to copy-paste Bash/POSIX snippets unchanged
- Config: `~/.config/fish/config.fish`; functions under `~/.config/fish/functions/`

## Quick facts

| Item       | Detail                                              |
| ---------- | --------------------------------------------------- |
| Package    | `fish`                                              |
| Binary     | `/usr/bin/fish`                                     |
| POSIX      | No — by design                                      |
| Web config | `fish_config` opens a browser UI for colors/prompts |

## Useful bits

```bash
fish --version
chsh -s $(which fish)
# set a universal variable

set -Ux EDITOR nvim
```

Syntax differs (`set` vs `export`, `if`/`end` blocks, no `$(( ))` Bash arithmetic the same way). For portable scripts, keep `#!/bin/sh` or `#!/bin/bash` files and use Fish interactively only.

## Related

- [Shells index](../README.md)
- [Bash](../bash/README.md)
- [Zsh](../zsh/README.md)
- [Terminals](../../terminals/README.md)
