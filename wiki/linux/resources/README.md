# Resources

Tags: `linux` `ops` `performance` `resources`

How Linux exposes and manages **CPU**, **memory**, and **disk** (plus process limits). Start with inspection commands, then tune only when you know what you're fixing.

## Pages

| Page                       | Focus                                     |
| -------------------------- | ----------------------------------------- |
| [Memory](memory/README.md) | RAM, swap, cache, OOM                     |
| [CPU](cpu/README.md)       | Load, cores, pressure, nice/affinity      |
| [Disk](disk/README.md)     | Capacity, inodes, I/O, mounts             |
| [Limits](limits/README.md) | ulimit, cgroups, systemd resource control |

## Quick health check

```bash
free -h
uptime
df -h
df -hi
iostat -xz 1        # if sysstat installed
```

## Related

- [Systemd](../systemd/README.md) — services and resource controls
- [Kernel / sysctl](../linux-kernel/sysctl/README.md)
- [Docker container limits](../../containers/docker/containers/README.md)
- [Linux index](../README.md)
