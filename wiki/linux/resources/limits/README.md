# Limits

Tags: `cgroups` `limits` `linux` `ops` `resources` `ulimit`

Caps on what a process (or group of processes) may consume — so one job can't take the whole machine.

## Shell ulimit (per process)

```bash
ulimit -a
ulimit -n          # open files
ulimit -u          # max user processes
```

Soft vs hard limits: soft can be raised up to hard (within policy). Services often set limits in **systemd** unit files, not your interactive shell.

```ini
# example snippet in a .service

[Service]
LimitNOFILE=65535
MemoryMax=512M
CPUQuota=200%
```

See [Systemd](../../systemd/README.md).

## cgroups

**Control groups** account for and limit CPU, memory, I/O for a process tree. systemd and containers both use cgroups (v2 on modern distros).

```bash
# often

ls /sys/fs/cgroup
systemctl status ssh
# memory of a service's cgroup (paths vary)

```

You rarely edit cgroups by hand on a desktop — use systemd, Docker/Podman, or orchestrators.

## Containers

Docker/Podman flags map onto cgroups:

```bash
docker run -m 512m --cpus=1.5 …
podman run -m 512m --cpus=1.5 …
```

See [Docker containers](../../../containers/docker/containers/README.md).

## When to set limits

| Goal                                 | Approach                           |
| ------------------------------------ | ---------------------------------- |
| Protect host from a hungry service   | systemd `MemoryMax=` / `CPUQuota=` |
| Fair share on a shared box           | cgroups / container limits         |
| Raise "too many open files"          | `LimitNOFILE` / `ulimit -n`        |
| Stop one container eating the laptop | `--memory` / `--cpus`              |

## Related

- [Memory](../memory/README.md) — OOM vs limits
- [CPU](../cpu/README.md)
- [Systemd](../../systemd/README.md)
- [Resources index](../README.md)
