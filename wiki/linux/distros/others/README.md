# Others

Tags: `distro` `distros` `linux` `others`

Index of distros outside the Debian/Ubuntu, Fedora/RHEL, and Arch families. Each has its own page.

## Pages

| Distro                | Page                             |
| --------------------- | -------------------------------- |
| openSUSE / SUSE       | [opensuse](opensuse/README.md)   |
| Alpine Linux          | [alpine](alpine/README.md)       |
| NixOS                 | [nixos](nixos/README.md)         |
| Gentoo                | [gentoo](gentoo/README.md)       |
| Void Linux            | [void](void/README.md)           |
| Slackware             | [slackware](slackware/README.md) |
| Immutable / specialty | [specialty](specialty/README.md) |

## Containers aren't distros (but feel like them)

Base images (`ubuntu`, `debian`, `alpine`, `rockylinux`) are how many people "choose a distro" for apps. Prefer slim official images and pin tags for production — see [Docker images](../../../containers/docker/images/README.md).

## How to identify any system

```bash
cat /etc/os-release
uname -r
hostnamectl          # systemd systems
lsb_release -a       # if installed
```

## Related

- [Distros index](../README.md)
- [Package managers](../../package-managers/README.md)
- [Linux index](../../README.md)
