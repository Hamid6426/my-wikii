# Nushell

Tags: `cli` `linux` `nushell` `shell` `shells` `shells-and-terminals`

**Nushell** is a modern shell that treats command output as **structured data** (tables) instead of only plain text streams. Pipelines feel closer to working with dataframes than classic Unix byte streams.

## Role

- Experimental / power-user interactive shell and scripting environment
- Strong when exploring JSON/CSV/system info as tables
- Not a POSIX `/bin/sh` replacement for system scripts

## Quick facts

| Item    | Detail                                       |
| ------- | -------------------------------------------- |
| Package | often `nushell` / `nu` (distro or cargo)     |
| Binary  | `nu`                                         |
| Config  | `~/.config/nushell/` (`config.nu`, `env.nu`) |
| Model   | Structured pipelines + built-in commands     |

## Useful bits

```bash
nu --version
# example mindset: ls returns a table you can filter/sort

# nu

# ls | where type == dir

```

Expect a learning curve: builtins and syntax differ from Bash. Use Nushell where structured data helps; keep Bash/POSIX for portable system automation.

## Related

- [Shells index](../README.md)
- [Bash](../bash/README.md)
- [Terminals](../../terminals/README.md)
