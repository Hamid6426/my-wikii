# MATE

Tags: `desktop` `desktop-environments` `gui` `linux` `mate`

GTK desktop continuing the GNOME 2 layout and workflow: panels, menu, clear traditional metaphors.

## Feel

Traditional, modest resources, predictable. Less "redesign churn" than GNOME Shell.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (GTK) + Caja, Pluma, …            │
├─────────────────────────────────────────┤
│  mate-panel · mate-desktop              │
├─────────────────────────────────────────┤
│  Marco (window manager)                 │
├─────────────────────────────────────────┤
│  X11 (primary)                          │
├─────────────────────────────────────────┤
│  mate-session · D-Bus                   │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴──────┐
      │ LightDM │  / GDM (varies)
      └─────────┘
```

| Layer           | Component                                                                                                         | Role                           |
| --------------- | ----------------------------------------------------------------------------------------------------------------- | ------------------------------ |
| Display manager | [LightDM](../../display-managers/lightdm/README.md) / [GDM](../../display-managers/gdm/README.md) (distro choice) | Login                          |
| Session         | **mate-session**                                                                                                  | Starts panel, WM, desktop      |
| Shell / UI      | **mate-panel**, mate background/desktop                                                                           | Classic panels and applets     |
| Window manager  | **Marco**                                                                                                         | Windows; optional compositor   |
| Toolkit         | **GTK 3** (MATE ports)                                                                                            | Apps and desktop UI            |
| Settings        | **mate-control-center**                                                                                           | Preferences                    |
| File manager    | **Caja**                                                                                                          | Nautilus fork from GNOME 2 era |

MATE is a component-for-component continuation of GNOME 2 naming (Caja ← Nautilus, Marco ← Metacity, etc.). Architecture stays classic multi-process DE, not a single Shell like GNOME 3+.

## Common distro homes

Ubuntu MATE, Linux Mint MATE, Fedora MATE spin, Debian MATE.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [XFCE](../xfce/README.md) — similar traditional niche
- [GNOME](../gnome/README.md) — modern GNOME lineage
