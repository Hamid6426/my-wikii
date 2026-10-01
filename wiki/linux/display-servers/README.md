# Display servers

Tags: `display-servers` `gui` `linux` `wayland` `x11`

The display server (or Wayland compositor) is what talks to the GPU and presents windows on screen.

## X11 (X.Org)

- Long-standing Linux graphics stack.
- Still widely supported; many apps and remote-desktop tools know it well.
- **XWayland** runs X11 apps under a Wayland session.

## Wayland

- Default on modern GNOME, Plasma, Sway, and Hyprland setups.
- Better isolation between clients; often smoother input and frame pacing.
- Some legacy apps or niche workflows still prefer X11.

## Check what you're on

```bash
echo $XDG_SESSION_TYPE    # wayland or x11
echo $XDG_CURRENT_DESKTOP
echo $DESKTOP_SESSION
```

## Related

- [Display managers](../display-managers/README.md) — login greeters (not the same layer)
- [Desktop environments](../desktop-environments/README.md)
- [Window managers](../window-managers/README.md)
- [Linux index](../README.md)
