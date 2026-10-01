# Debian

Tags: `apt` `deb` `debian` `distro` `distros` `linux` `ubuntu` `upstream`

Upstream "universal OS" and the base for Ubuntu and many other derivatives. Shares the `.deb` / `apt` ecosystem.

This page is nested under [Ubuntu](../README.md) only for wiki navigation — **Debian is not an Ubuntu flavor**; Ubuntu builds on Debian.

## Overview

- **Role:** Base for many derivatives; often used directly on servers.
- **Branches:** _stable_ (production), _testing_, _unstable_ (sid).
- **Strengths:** Conservative stability, clear policies, long-lived stable releases.
- **Trade-offs:** Older packages in stable; desktop polish often left to derivatives.
- **Typical use:** Servers, appliances, base for custom images.

```bash
sudo apt update
sudo apt upgrade
sudo apt install <package>
apt search <keyword>
```

Release info: `/etc/debian_version`, `/etc/os-release`.
