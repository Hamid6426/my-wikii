# Kernel modules

Tags: `drivers` `kernel` `linux` `linux-kernel` `modules`

Most drivers ship as **loadable modules** under `/lib/modules/$(uname -r)/`.

```bash
lsmod
modinfo <module>
sudo modprobe <module>
sudo modprobe -r <module>
```

## In-tree vs out-of-tree

- **In-tree:** built with the kernel; loaded on demand (udev/modprobe).
- **Out-of-tree:** NVIDIA, some VPN/virtualization tools — rebuild against **headers** when the kernel updates.

**DKMS** automates rebuilds on many distros after kernel upgrades. Keep at least one previous kernel in the boot menu for rollback.

## Related

- [Firmware](../firmware/README.md)
- [Boot](../boot/README.md)
- [Kernel index](../README.md)
