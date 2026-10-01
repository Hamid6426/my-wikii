# Package managers

Tags: `apt` `dnf` `linux` `package-managers` `packages` `pacman`

Cheat sheet across major Linux families. Prefer each distro's own docs for repos, pinning, and upgrades.

| Family          | Tool           | Install                    | Update indexes + upgrade                    | Search            | Remove                      |
| --------------- | -------------- | -------------------------- | ------------------------------------------- | ----------------- | --------------------------- |
| Debian / Ubuntu | `apt`          | `sudo apt install pkg`     | `sudo apt update && sudo apt upgrade`       | `apt search`      | `sudo apt remove` / `purge` |
| Fedora / RHEL   | `dnf`          | `sudo dnf install pkg`     | `sudo dnf upgrade`                          | `dnf search`      | `sudo dnf remove`           |
| Arch            | `pacman`       | `sudo pacman -S pkg`       | `sudo pacman -Syu`                          | `pacman -Ss`      | `sudo pacman -Rns`          |
| openSUSE        | `zypper`       | `sudo zypper install pkg`  | `sudo zypper refresh && sudo zypper update` | `zypper search`   | `sudo zypper remove`        |
| Alpine          | `apk`          | `sudo apk add pkg`         | `sudo apk update && sudo apk upgrade`       | `apk search`      | `sudo apk del`              |
| Void            | `xbps-install` | `sudo xbps-install -S pkg` | `sudo xbps-install -Su`                     | `xbps-query -Rs`  | `sudo xbps-remove`          |
| Gentoo          | `emerge`       | `sudo emerge pkg`          | `sudo emerge --sync` then emerge world      | `emerge --search` | `sudo emerge --unmerge`     |

## Universal / overlay formats

| Format       | Notes                                                    |
| ------------ | -------------------------------------------------------- |
| **Flatpak**  | Sandboxed desktop apps; common on Fedora/Silverblue      |
| **Snap**     | Ubuntu-default for some apps; controversial elsewhere    |
| **AppImage** | Self-contained single file; little integration           |
| **Nix**      | Works on NixOS and as a package manager on other distros |

## Related

- [Distros](../distros/README.md)
- [Containers](../../containers/README.md) — container images vs host packages
- [Linux index](../README.md)
