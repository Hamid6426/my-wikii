# Boot path

Tags: `boot` `bootloader` `initramfs` `kernel` `linux` `linux-kernel`

Typical Linux boot:

1. Firmware (UEFI/BIOS) → bootloader (**GRUB**, systemd-boot, …)
2. Bootloader loads **kernel + initramfs**
3. Initramfs mounts real root (may unlock LUKS)
4. **PID 1** (usually [systemd](../../systemd/README.md)) starts userspace

## Kernel cmdline

Often set in GRUB (`GRUB_CMDLINE_LINUX` / `/etc/default/grub` then `update-grub` / `grub2-mkconfig`).

Common parameters: `root=`, `quiet`, `nomodeset`, `mitigations=`, `rd.`* for initramfs.

```bash
cat /proc/cmdline
```

## Related

- [Modules](../modules/README.md)
- [Systemd](../../systemd/README.md)
- [Kernel index](../README.md)
