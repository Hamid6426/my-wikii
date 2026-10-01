# Disk

Tags: `disk` `filesystem` `linux` `ops` `resources` `storage`

Disk "problems" are usually **space**, **inodes**, or **I/O latency** — not just "is the SSD full?"

## Capacity

```bash
df -h              # human sizes
df -hT             # include filesystem type
du -sh /var/*      # directory usage
du -xh --max-depth=1 /var | sort -h
```

| Symptom                           | Check                            |
| --------------------------------- | -------------------------------- |
| "No space" but `df -h` shows free | **Inodes** exhausted — `df -hi`  |
| Full `/` or `/var`                | Logs, containers, package caches |
| Full `/boot`                      | Too many old kernels             |

```bash
df -hi
# cleanup ideas (careful):

sudo journalctl --vacuum-size=200M
docker system df          # if Docker used
```

## Mounts

```bash
findmnt
lsblk -f
mount | column -t
```

Know whether data lives on `/`, a separate `/home`, network mounts, or bind mounts (containers often confuse this).

## I/O performance

```bash
iostat -xz 1          # sysstat
sudo iotop            # per-process I/O (if installed)
vmstat 1
```

High **`%util`** / long **await** → disk saturated or slow. CPU **`wa`** (iowait) in `top` often points here, not "need more GHz."

## Filesystem notes

- **ext4 / xfs / btrfs** — different features (snapshots on btrfs, etc.).
- Don't run `fsck` on mounted read-write root casually — use recovery/maintenance flows.
- RAID/LVM add layers (`lsblk`, `lvs`, `vgs`) — capacity shown by `df` is the filesystem, not always the raw disk.

## Related

- [Memory](../memory/README.md) — swap is disk-backed
- [CPU](../cpu/README.md)
- [Limits](../limits/README.md)
- [Docker disk / prune](../../../containers/docker/cli/README.md)
- [Resources index](../README.md)
