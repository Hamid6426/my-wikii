# Budgie

Tags: `budgie` `desktop` `desktop-environments` `gui` `linux`

> **Version drift:** Budgie 10.x vs newer compositor stacks differ by distro package — confirm your release's WM/session packages.

GTK desktop known for a clean panel layout and the Raven sidebar (notifications / applets). Simple defaults, modern look.

## Feel

Clean and approachable without GNOME Shell's overview model.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (GTK) + Nautilus or alternatives  │
├─────────────────────────────────────────┤
│  budgie-panel · Raven · budgie-desktop  │
├─────────────────────────────────────────┤
│  Magpie / Mutter-based WM+compositor    │
│  (Budgie 10.x lineage evolved here)     │
├─────────────────────────────────────────┤
│  X11 / Wayland (depends on release)     │
├─────────────────────────────────────────┤
│  budgie-session / labwc paths · D-Bus   │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴──────┐
      │ LightDM │  / GDM (varies)
      └─────────┘
```

| Layer           | Component                                                                                                  | Role                                           |
| --------------- | ---------------------------------------------------------------------------------------------------------- | ---------------------------------------------- |
| Display manager | [LightDM](../../display-managers/lightdm/README.md) / [GDM](../../display-managers/gdm/README.md) (distro) | Login                                          |
| Session         | Budgie session services                                                                                    | Starts panel and desktop                       |
| Shell / UI      | **budgie-panel**, **Raven**                                                                                | Panel applets; sidebar for notes/notifications |
| WM + compositor | Historically **budgie-wm** (Mutter-based); newer Budgie work targets modern compositor stacks              | Windows and effects                            |
| Toolkit         | **GTK**                                                                                                    | UI and many apps                               |
| Settings        | Budgie Desktop Settings + GNOME/GTK settings where reused                                                  | Preferences                                    |
| File manager    | Often **Nautilus** or distro default                                                                       | Files                                          |

Budgie reuses pieces of the GNOME ecosystem (GTK, some session tech) but replaces GNOME Shell with its own panel/Raven UX. Exact WM/compositor package names shift across Budgie 10 → newer releases — check your distro's Budgie version docs.

## Common distro homes

Ubuntu Budgie, Solus (historically), packages on Arch and others.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [GNOME](../gnome/README.md) — shared GTK / GNOME tech roots
