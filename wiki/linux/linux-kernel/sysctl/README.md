# Sysctl

Tags: `kernel` `linux` `linux-kernel` `sysctl` `tuning`

**sysctl** exposes runtime kernel knobs under `/proc/sys`. Changes can be temporary or persisted in `/etc/sysctl.d/*.conf`.

```bash
sysctl -a | less
sysctl net.ipv4.ip_forward
sudo sysctl -w net.ipv4.ip_forward=1
```

Persist:

```bash
echo 'net.ipv4.ip_forward = 1' | sudo tee /etc/sysctl.d/99-forward.conf
sudo sysctl --system
```

Common areas: networking (`net.*`), VM/memory (`vm.*`), kernel (`kernel.*`). Prefer distro/security docs before copying random internet tunables — especially on shared hosts and with [Docker](../../../containers/docker/README.md) (which also manipulates networking).

## Related

- [Linux networking](../../networking/README.md)
- [Resources](../../resources/README.md) — memory/CPU/disk when tuning `vm.*`
- [Docker networking](../../../containers/docker/networking/README.md)
- [Kernel index](../README.md)
