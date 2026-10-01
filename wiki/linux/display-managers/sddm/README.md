# SDDM

Tags: `display-managers` `dm` `gui` `kde` `linux` `sddm`

**Simple Desktop Display Manager** — Qt-based greeter. Default on most KDE Plasma installs; also common with LXQt and other Qt-friendly setups.

## Feel

Themeable Qt UI, clear session list. Natural fit next to Plasma; works fine for non-Plasma sessions too.

## Stack

- **Typical DE:** [KDE Plasma](../../desktop-environments/kde-plasma/README.md), often [LXQt](../../desktop-environments/lxqt/README.md)
- **Config:** `/etc/sddm.conf` and `/etc/sddm.conf.d/`
- **systemd unit:** often `sddm.service`

## Tips

- Themes and DPI/scaling knobs live in SDDM config / theme packages — separate from Plasma’s in-session settings.
- Wayland vs X11 Plasma sessions show up as different entries when both are installed.

## Related

- [Display managers index](../README.md)
- [KDE Plasma](../../desktop-environments/kde-plasma/README.md)
- [LXQt](../../desktop-environments/lxqt/README.md)
- [Display servers](../../display-servers/README.md)
- [GDM](../gdm/README.md) · [LightDM](../lightdm/README.md)
