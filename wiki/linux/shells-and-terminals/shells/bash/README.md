# Bash

Tags: `bash` `cli` `linux` `shell` `shells` `shells-and-terminals`

**Bash** (Bourne Again SHell) is the default interactive shell on most Linux distributions. It is largely Bourne/`sh`-compatible with a large set of interactive and scripting extensions.

## Role

- Interactive CLI on Ubuntu, Fedora, Debian, many others
- De-facto scripting language for system scripts (though `/bin/sh` may be Dash — see [Dash](../dash/README.md))
- Config: `~/.bashrc` (interactive non-login), `~/.bash_profile` / `~/.profile` (login — distro-dependent)

## Quick facts

| Item       | Detail                                    |
| ---------- | ----------------------------------------- |
| Package    | `bash`                                    |
| Binary     | `/bin/bash` or `/usr/bin/bash`            |
| POSIX      | Superset of POSIX sh (has Bashisms)       |
| Completion | bash-completion package common on distros |

## Useful bits

```bash
echo $BASH_VERSION
# interactive config

nano ~/.bashrc
# set as login shell

chsh -s /bin/bash
```

Common interactive features: history (`!!`, `Ctrl+R`), aliases, functions, programmable completion, job control.

## Scripting caution

A script with `#!/bin/bash` may use arrays, `[[ ]]`, process substitutions, etc. A script with `#!/bin/sh` should stay POSIX-portable — on Debian/Ubuntu `/bin/sh` is often **Dash**, not Bash.

## Related

- [Shells index](../README.md)
- [Zsh](../zsh/README.md)
- [Dash](../dash/README.md)
- [Terminals](../../terminals/README.md)
