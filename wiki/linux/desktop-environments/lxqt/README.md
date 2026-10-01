# LXQt

Tags: `desktop` `desktop-environments` `gui` `linux` `lxqt`

Lightweight Qt desktop. Spiritual successor direction to LXDE, keeping a simple panel desktop with low overhead.

## Feel

Very light. Minimal bundled "opinion"; you assemble extras as needed.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (Qt) + PCManFM-Qt, editors…       │
├─────────────────────────────────────────┤
│  lxqt-panel · lxqt-runner · desktop     │
├─────────────────────────────────────────┤
│  Window manager (swappable)             │
│    default often: Openbox               │
├─────────────────────────────────────────┤
│  X11 (typical)                          │
├─────────────────────────────────────────┤
│  lxqt-session · D-Bus                   │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴────────────┐
      │ SDDM / LightDM │
      └───────────────┘
```

| Layer           | Component                                                                                                    | Role                                  |
| --------------- | ------------------------------------------------------------------------------------------------------------ | ------------------------------------- |
| Display manager | **[SDDM](../../display-managers/sddm/README.md)** or **[LightDM](../../display-managers/lightdm/README.md)** | Login                                 |
| Session         | **lxqt-session**                                                                                             | Starts panel, WM, policykit helper    |
| Shell / UI      | **lxqt-panel**, lxqt desktop components                                                                      | Panel, menu, widgets                  |
| Window manager  | **Openbox** (common default), or others                                                                      | LXQt does **not** ship a mandatory WM |
| Toolkit         | **Qt**                                                                                                       | Desktop and apps                      |
| Settings        | **lxqt-config**                                                                                              | Configuration center                  |
| File manager    | **PCManFM-Qt**                                                                                               | Files / folders                       |

Key architectural point: LXQt is a **desktop layer on top of a separate WM**. Swapping Openbox for kwin_x11 or another WM is normal.

## Common distro homes

Lubuntu, Fedora LXQt spin, Debian LXQt, some Arch community setups.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [Openbox](../../window-managers/openbox/README.md) — common WM underneath
- [KDE Plasma](../kde-plasma/README.md) — fuller Qt desktop
