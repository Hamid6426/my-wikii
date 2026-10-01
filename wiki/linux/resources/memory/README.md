# Memory

Tags: `linux` `memory` `oom` `ops` `performance` `resources`

Linux memory management: physical RAM, reclaimable **cache/buffers**, **swap**, and the **OOM killer** when the system is out of options.

## Inspect

```bash
free -h
cat /proc/meminfo | head
# per-process

ps aux --sort=-%mem | head
# interactive

htop          # or top, then shift+M
```

| `free` column  | Meaning                                                                                   |
| -------------- | ----------------------------------------------------------------------------------------- |
| **used**       | Memory in active use by processes (roughly)                                               |
| **buff/cache** | Kernel cache — often large; **can be reclaimed** when apps need RAM                       |
| **available**  | Estimate of memory available for new workloads without swapping (best "is it OK?" number) |
| **swap**       | Overflow on disk — slow; some swap is normal, constant heavy swap is a problem            |

Don't panic because "cache looks huge." Prefer **available** over a naive `used/total` reading.

## Swap

```bash
swapon --show
cat /proc/swaps
```

- No/low swap → OOM risk under spikes (desktops sometimes use zram instead).
- Thrashing (high swap I/O + high wait) → add RAM or reduce workload.

## Pressure & reclaim

Modern kernels expose pressure stalls (PSI) on many systems:

```bash
# if present

cat /proc/pressure/memory
```

High memory pressure → tasks stall waiting for reclaim.

## OOM killer

When reclaim fails, the kernel may kill a process (OOM). Check:

```bash
dmesg -T | grep -i oom
journalctl -k | grep -i oom
```

Fix: less memory use, more RAM/swap, or cgroup limits so one job can't eat the host — see [Limits](../limits/README.md).

## Related

- [CPU](../cpu/README.md)
- [Disk](../disk/README.md)
- [Limits](../limits/README.md)
- [Resources index](../README.md)
