# Cinnamon

Tags: `cinnamon` `desktop` `desktop-environments` `gui` `linux`

GTK desktop from the Linux Mint project. Traditional layout (panel, menu, applets) with modern features and strong out-of-box polish on Mint.

## Feel

Familiar traditional desktop — often comfortable for users coming from Windows.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps (GTK) + Nemo, apps…               │
├─────────────────────────────────────────┤
│  Cinnamon Shell (panel, menu, applets)  │
├─────────────────────────────────────────┤
│  Muffin (Mutter fork: WM + compositor)  │
├─────────────────────────────────────────┤
│  X11 (typical) / Wayland efforts        │
├─────────────────────────────────────────┤
│  cinnamon-session · D-Bus               │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴──────┐
      │ LightDM │  (Mint default)
      └─────────┘
```

| Layer           | Component                                                            | Role                                  |
| --------------- | -------------------------------------------------------------------- | ------------------------------------- |
| Display manager | **[LightDM](../../display-managers/lightdm/README.md)** (Linux Mint) | Login                                 |
| Session         | **cinnamon-session**                                                 | Session startup                       |
| Shell / UI      | **Cinnamon** (JS shell)                                              | Panel, menu, expo, applets/desklets   |
| WM + compositor | **Muffin**                                                           | Fork of Mutter; windows + effects     |
| Toolkit         | **GTK**                                                              | Apps and Cinnamon UI stack            |
| Settings        | **Cinnamon Settings**                                                | Desktop and system prefs              |
| File manager    | **Nemo**                                                             | Fork of Nautilus; Mint's file manager |

Cinnamon keeps a GNOME 3–era architecture (Shell + Mutter-derived WM) but restores a traditional desktop metaphor. Spices (applets/extensions/themes) plug into the shell.

## Common distro homes

Linux Mint (flagship), also installable on Ubuntu/Debian-based systems and others via packages.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [GNOME](../gnome/README.md) — upstream Shell/Mutter lineage
- [Ubuntu / Mint context](../../distros/ubuntu/README.md)
