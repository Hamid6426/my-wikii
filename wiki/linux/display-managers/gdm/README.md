# GDM

Tags: `display-managers` `dm` `gdm` `gui` `linux`

**GNOME Display Manager** — the usual login greeter for GNOME. Handles authentication and starts GNOME (or other listed) sessions; tightly integrated with the GNOME stack.

## Feel

Polished GNOME-style greeter. Best match when the machine’s main desktop is GNOME; other sessions can still appear in the session list if their packages install `.desktop` entries.

## Stack

- **Typical DE:** [GNOME](../../desktop-environments/gnome/README.md)
- **Sessions:** Wayland default on modern GNOME; X11 where still shipped
- **systemd unit:** often `gdm.service` (aliased as `display-manager.service` when selected)

## Tips

- Session chooser is on the greeter (gear / menu) before you sign in.
- Problems logging into Wayland vs X11 are often session selection or GPU driver issues, not GDM itself — check `$XDG_SESSION_TYPE` after login.

## Related

- [Display managers index](../README.md)
- [GNOME](../../desktop-environments/gnome/README.md)
- [Display servers](../../display-servers/README.md)
- [SDDM](../sddm/README.md) · [LightDM](../lightdm/README.md)
