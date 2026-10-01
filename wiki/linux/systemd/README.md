# Systemd

Tags: `init` `linux` `services` `systemd` `units`

**systemd** is the mainstream Linux init system and service manager (PID 1 on most desktops/servers). Exceptions: Alpine (OpenRC), Void (runit), some Gentoo setups.

## Everyday commands

```bash
systemctl status ssh
sudo systemctl start ssh
sudo systemctl stop ssh
sudo systemctl restart ssh
sudo systemctl enable --now ssh    # start + enable on boot
sudo systemctl disable ssh

systemctl list-units --type=service --state=running
systemctl cat ssh
```

## Journals (logs)

```bash
journalctl -u ssh -e
journalctl -b          # this boot
journalctl -f          # follow
journalctl --since "1 hour ago"
```

Kernel messages: `journalctl -k` (see also [kernel](../linux-kernel/README.md)).

## Unit basics

Units live under `/etc/systemd/system/` (admin) and `/lib/systemd/system/` (packages). After editing:

```bash
sudo systemctl daemon-reload
sudo systemctl restart myapp.service
```

Common types: `.service`, `.timer`, `.socket`, `.mount`, `.target`.

## Related

- [Linux kernel / boot](../linux-kernel/boot/README.md)
- [Resources / limits](../resources/limits/README.md) — MemoryMax, CPUQuota, ulimit
- [Docker logging](../../containers/docker/cli/logging/README.md) — `journald` driver
- [Distros](../distros/README.md)
- [Linux index](../README.md)
