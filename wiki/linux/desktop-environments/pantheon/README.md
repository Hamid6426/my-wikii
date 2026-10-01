# Pantheon

Tags: `desktop` `desktop-environments` `gui` `linux` `pantheon`

Opinionated desktop shipped with elementary OS. Polished, mac-like aesthetics and curated app set (AppCenter culture).

## Feel

Cohesive and minimal chrome. Fewer "infinite settings" than Plasma; design-led defaults.

## Architecture

```
┌─────────────────────────────────────────┐
│  elementary apps (Granite + GTK)        │
├─────────────────────────────────────────┤
│  Wingpanel · Dock · Applist / launcher  │
├─────────────────────────────────────────┤
│  Gala (Mutter-based WM + compositor)    │
├─────────────────────────────────────────┤
│  Wayland / X11 (per elementary release) │
├─────────────────────────────────────────┤
│  elementary session · D-Bus             │
└─────────────────────────────────────────┘
         ▲
         │
      ┌──┴──────┐
      │ LightDM │  (elementary)
      └─────────┘
```

| Layer           | Component                                               | Role                                          |
| --------------- | ------------------------------------------------------- | --------------------------------------------- |
| Display manager | **[LightDM](../../display-managers/lightdm/README.md)** | Login (elementary greeter)                    |
| Session         | elementary session packages                             | Starts Gala, Wingpanel, Dock                  |
| Shell / UI      | **Wingpanel**, **Dock**, applications menu              | Top panel and dock (mac-like layout)          |
| WM + compositor | **Gala**                                                | Mutter-based; windows, workspaces, animations |
| Toolkit / libs  | **GTK** + **Granite**                                   | Widgets and elementary styling                |
| Settings        | **Switchboard**                                         | Pluggable settings panels ("plugs")           |
| File manager    | **Files** (Pantheon Files)                              | Files / folders                               |
| App store       | **AppCenter**                                           | Curated app distribution                      |

Pantheon is tightly integrated as a product on elementary OS: Gala handles the compositor role while Wingpanel/Dock provide the chrome instead of GNOME Shell.

## Common distro homes

elementary OS primarily; community ports exist but the full experience is tied to elementary.

## Related

- [Desktop environments index](../README.md)
- [Display servers](../../display-servers/README.md)
- [GNOME](../gnome/README.md) — Mutter lineage via Gala
- [Ubuntu family](../../distros/ubuntu/README.md) — elementary is Ubuntu-based
