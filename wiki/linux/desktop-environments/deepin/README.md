# Deepin (DDE)

Tags: `deepin` `desktop` `desktop-environments` `gui` `linux`

Deepin Desktop Environment — polished, effects-heavy desktop from the Deepin project. Distinct look with its own control center and apps.

## Feel

Visually refined; can feel heavier than XFCE/LXQt. Strong out-of-box theming.

## Architecture

```
┌─────────────────────────────────────────┐
│  DDE apps + deepin-* utilities          │
├─────────────────────────────────────────┤
│  dde-dock · dde-launcher · desktop      │
├─────────────────────────────────────────┤
│  deepin-kwin / KWin-based stack         │
│  (DDE builds on KDE tech in modern DDE) │
├─────────────────────────────────────────┤
│  X11 / Wayland (per deepin release)     │
├─────────────────────────────────────────┤
│  startdde · D-Bus · deepin daemon set   │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴────────────┐
      │ lightdm-deepin │  (typical on deepin OS)
      └───────────────┘
```

| Layer           | Component                                                                               | Role                         |
| --------------- | --------------------------------------------------------------------------------------- | ---------------------------- |
| Display manager | **[LightDM](../../display-managers/lightdm/README.md)** + deepin greeter (on deepin OS) | Login                        |
| Session         | **startdde** / DDE session                                                              | Boots dock, desktop, daemons |
| Shell / UI      | **dde-dock**, launcher, desktop modules                                                 | Taskbar/dock and launcher    |
| WM + compositor | **deepin-kwin** (KWin-derived in current DDE generations)                               | Windows and effects          |
| Toolkit         | **Qt** (+ DTK — Deepin Tool Kit)                                                        | DDE apps and controls        |
| Settings        | **Control Center** (`dde-control-center`)                                               | Central preferences          |
| File manager    | **deepin-file-manager**                                                                 | Files / folders              |

Modern DDE leans on a **Qt + KWin-family** architecture with Deepin's own DTK widgets and dock/launcher replacing Plasma Shell. Older deepin generations differed — verify against the major version you run.

## Notes

Packaging quality outside deepin's own OS varies — check your distro's repos or official deepin guidance.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [KDE Plasma](../kde-plasma/README.md) — related KWin/Qt ecosystem
