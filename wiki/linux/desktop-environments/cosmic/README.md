# COSMIC

Tags: `cosmic` `desktop` `desktop-environments` `gui` `linux` `wayland`

> **Moving fast:** COSMIC APIs, package names, and Pop!_OS integration change between releases — prefer System76 / Pop!_OS docs for install steps. Notes here are conceptual.

Desktop environment from System76, written with a Rust-heavy stack. Evolving alongside Pop!_OS as the successor to their GNOME-based desktop — tiling-friendly and modern.

## Feel

Still maturing; aims at productivity layouts (including tiling) with a cohesive System76 design.

## Architecture

```
┌─────────────────────────────────────────┐
│  Apps + COSMIC applets / libcosmic UI   │
├─────────────────────────────────────────┤
│  cosmic-panel · cosmic-launcher · …     │
├─────────────────────────────────────────┤
│  cosmic-comp (Wayland compositor)       │
├─────────────────────────────────────────┤
│  Wayland (first-class)                  │
├─────────────────────────────────────────┤
│  cosmic-session · cosmic-greeter        │
└─────────────────────────────────────────┘
```

| Layer             | Component                                                   | Role                                                            |
| ----------------- | ----------------------------------------------------------- | --------------------------------------------------------------- |
| Greeter / DM      | **cosmic-greeter** (and display-manager integration)        | Login                                                           |
| Session           | **cosmic-session**                                          | Composes the COSMIC user session                                |
| Shell / UI        | **cosmic-panel**, launcher, applets, settings               | Desktop chrome                                                  |
| Compositor        | **cosmic-comp**                                             | Wayland compositor + window management (incl. tiling workflows) |
| Toolkit / UI libs | **libcosmic** / iced-based Rust UI stack                    | Native COSMIC apps and widgets                                  |
| Settings          | **cosmic-settings**                                         | System and desktop configuration                                |
| File manager      | COSMIC Files (and/or distro defaults while ecosystem grows) | Files                                                           |

COSMIC is a **from-scratch DE**, not a GNOME Shell skin: compositor, session, and UI are System76 projects sharing a Rust-centric architecture. Details move quickly — treat System76 / Pop!_OS docs as source of truth for package names.

## Notes

Specs and packaging change quickly while COSMIC stabilizes — prefer System76 / Pop!_OS docs for install and upgrade paths.

## Related

- [Desktop environments index](../README.md)
- [Display managers](../../display-managers/README.md) — cosmic-greeter / DM integration
- [Display servers](../../display-servers/README.md)
- [GNOME](../gnome/README.md) — previous Pop!_OS default
- [Pop!_OS / Ubuntu family](../../distros/ubuntu/README.md)
