# CPU

Tags: `cgroups` `cpu` `linux` `ops` `performance` `resources`

CPU capacity is about **cores/threads**, how busy they are, and whether the run queue is backed up (**load average**).

## Inspect

```bash
nproc
lscpu
uptime                    # load average 1/5/15 min
mpstat -P ALL 1           # if sysstat installed
ps aux --sort=-%cpu | head
htop                      # or top
```

## Load average

`uptime` shows three numbers (1, 5, 15 minutes). Rough rule of thumb:

- Compare to **CPU count** (`nproc`).
- Load ≈ number of runnable/uninterruptible tasks.
- Load **persistently >> nproc** → CPU contention or heavy disk wait (uninterruptible sleep).

Load alone doesn't say "CPU bound" vs "I/O stuck" — check `top`/`vmstat`/`iostat`.

## Steal & virtualization

On VMs, **`%st` (steal)** in `top`/`mpstat` means the hypervisor took time for other guests — your vCPU isn't fully yours.

## Nice and priority

```bash
nice -n 10 ./batch-job
renice -n 10 -p <pid>
```

Higher nice → lower priority (nicer to others). Realtime priorities exist but are advanced/dangerous on desktops.

## Affinity (pin to CPUs)

```bash
taskset -c 0,1 ./app
taskset -p <pid>
```

Useful for isolating noisy workloads; usually unnecessary for casual use.

## Related

- [Memory](../memory/README.md)
- [Disk](../disk/README.md) — I/O wait can look like "CPU problems"
- [Limits](../limits/README.md)
- [Resources index](../README.md)
