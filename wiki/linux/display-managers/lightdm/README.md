# LightDM

Tags: `display-managers` `dm` `gui` `lightdm` `linux`

Cross-DE display manager with swappable **greeters**. Common default on XFCE, Cinnamon (Linux Mint), MATE, Pantheon/elementary, and many Ubuntu flavors.

## Feel

Lightweight and flexible. The look depends on the greeter package (GTK, slick-greeter, pantheon, deepin, …), not on LightDM alone.

## Stack

- **Typical DEs:** [XFCE](../../desktop-environments/xfce/README.md), [Cinnamon](../../desktop-environments/cinnamon/README.md), [MATE](../../desktop-environments/mate/README.md), [Pantheon](../../desktop-environments/pantheon/README.md), [Budgie](../../desktop-environments/budgie/README.md), [Deepin](../../desktop-environments/deepin/README.md)
- **Config:** `/etc/lightdm/lightdm.conf` (+ greeter-specific conf)
- **systemd unit:** often `lightdm.service`

## Tips

- If the greeter looks wrong after a DE change, check which greeter package is installed and what `greeter-session=` points at.
- Supports X11 sessions widely; Wayland support depends on greeter and distro packaging.

## Related

- [Display managers index](../README.md)
- [Desktop environments](../../desktop-environments/README.md)
- [Display servers](../../display-servers/README.md)
- [GDM](../gdm/README.md) · [SDDM](../sddm/README.md) · [greetd](../greetd/README.md)
