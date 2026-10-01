# Dash / POSIX `sh`

Tags: `cli` `dash` `linux` `posix` `shell` `shells` `shells-and-terminals`

**Dash** (Debian Almquist Shell) is a small, fast POSIX shell. On Debian and Ubuntu, **`/bin/sh` is typically Dash**, not Bash — important for script portability.

## Role

- `/bin/sh` target for system scripts that should be POSIX
- Faster startup and fewer features than Bash (no arrays, limited builtins)
- You rarely use Dash as your interactive daily shell

## Quick facts

| Item        | Detail                                       |
| ----------- | -------------------------------------------- |
| Package     | `dash`                                       |
| Binary      | `/bin/dash`; often linked as `/bin/sh`       |
| POSIX       | Aimed at POSIX compliance                    |
| Interactive | Minimal — prefer Bash/Zsh/Fish for daily use |

## Useful bits

```bash
ls -l /bin/sh
# shebang for portable scripts

head -1 /etc/os-release
# write scripts as:

# #!/bin/sh

```

If a script breaks with `#!/bin/sh` but works with `#!/bin/bash`, it likely uses **Bashisms** (`[[ ]]`, arrays, `source` vs `.` nuances, etc.).

## Related

- [Shells index](../README.md)
- [Bash](../bash/README.md)
- [Terminals](../../terminals/README.md)
