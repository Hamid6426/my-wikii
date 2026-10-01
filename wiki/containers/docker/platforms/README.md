# Platforms

Tags: `containers` `docker` `install` `oci` `platforms`

How Docker runs depends on the **host OS**. Linux containers need a Linux kernel — natively on Linux, via WSL2 on Windows, or via a VM on macOS.

## Pages

| Host    | How Docker usually runs                                               | Page                         |
| ------- | --------------------------------------------------------------------- | ---------------------------- |
| Linux   | Engine on the host kernel                                             | [linux](linux/README.md)     |
| Windows | Docker Desktop + WSL2 (Linux containers); optional Windows containers | [windows](windows/README.md) |
| macOS   | Docker Desktop + a Linux VM                                           | [macos](macos/README.md)     |

```
┌──────────┐   ┌───────────────┐   ┌────────────┐
│  Linux   │   │   Windows     │   │   macOS    │
│  kernel  │   │ WSL2 / Hyper-V│   │  Linux VM  │
└────┬─────┘   └───────┬───────┘   └─────┬──────┘
     │                 │                 │
     └───────────── dockerd ─────────────┘
                      │
                 containers
```

## Related

- [Docker index](../README.md)
