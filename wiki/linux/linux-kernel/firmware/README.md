# Firmware

Tags: `firmware` `hardware` `kernel` `linux` `linux-kernel`

Missing Wi-Fi/GPU features are often **firmware blobs**, not "wrong distro."

```bash
# Debian/Ubuntu family examples

sudo apt install linux-firmware

# Check kernel messages for firmware load failures

sudo dmesg | grep -i firmware
```

Package names vary (`linux-firmware`, vendor-specific packages). Secure Boot can also block out-of-tree modules even when firmware is present.

## Related

- [Modules](../modules/README.md)
- [Kernel index](../README.md)
- [Distros](../../distros/README.md)
