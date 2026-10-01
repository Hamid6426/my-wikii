# Linux kernel

Tags: `kernel` `linux` `linux-kernel`

The Linux kernel is the core of the OS: hardware access, processes, memory, filesystems, networking, and drivers. Distros package a kernel (or several), ship modules/firmware, and decide update cadence — the kernel itself is shared upstream from [kernel.org](https://www.kernel.org/).

## Pages

| Page                           | Focus                                              |
| ------------------------------ | -------------------------------------------------- |
| [Modules](modules/README.md)   | `lsmod`, modprobe, DKMS, out-of-tree drivers       |
| [Boot](boot/README.md)         | Firmware → bootloader → kernel → initramfs → PID 1 |
| [Firmware](firmware/README.md) | `linux-firmware`, dmesg failures, Wi-Fi/GPU blobs  |
| [Sysctl](sysctl/README.md)     | Runtime kernel knobs via `/proc/sys`               |

## What a distro ships

| Piece                  | Role                                                       |
| ---------------------- | ---------------------------------------------------------- |
| **vmlinuz**            | Compressed kernel image loaded by the bootloader           |
| **initramfs / initrd** | Early userspace to find root FS, unlock LUKS, load modules |
| **modules**            | Loadable drivers under `/lib/modules/$(uname -r)/`         |
| **firmware**           | Binary blobs for Wi-Fi, GPU, etc.                          |
| **headers**            | Needed to build out-of-tree modules (NVIDIA, DKMS)         |

## Versioning (quick)

- Upstream versions look like `6.12`, `6.13`, … — major.minor (and patch).
- Distros add their own packaging suffix, e.g. `6.12.0-25-generic` (Ubuntu) or `6.14.4-200.fc41.x86_64` (Fedora).
- **LTS kernels** are popular on servers; some distros ship **HWE** / newer kernels on stable releases.
- Distro kernels include patches and configs chosen by that project.

## Useful commands

```bash
uname -r
uname -a
cat /proc/version
hostnamectl
ls /lib/modules
journalctl -k -b
dmesg | less
```

## Building / custom kernels

Most users never compile a kernel. Prefer distro packages unless you need a custom config, staging driver, or appliance image (Yocto/Buildroot).

## Related

- [Distros](../distros/README.md)
- [Systemd](../systemd/README.md)
- [Desktop environments](../desktop-environments/README.md)
- [Docker on Linux](../../containers/docker/platforms/linux/README.md)
- [Linux index](../README.md)
