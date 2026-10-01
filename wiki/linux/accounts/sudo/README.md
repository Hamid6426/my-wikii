# Sudo

Tags: `accounts` `identity` `linux` `privileges` `security` `sudo`

**sudo** lets an allowed user run individual commands as **root** (or another user) without staying logged in as root.

## Basics

```bash
sudo apt update                 # run as root
sudo -u www-data whoami         # run as another user
sudo -i                         # interactive root shell
sudo -l                         # list your privileges
```

First time (and periodically) sudo asks for **your** password (not root's), unless configured otherwise.

## Who may sudo?

Membership in a group, or an explicit sudoers rule:

| Distro family   | Common admin group |
| --------------- | ------------------ |
| Debian / Ubuntu | `sudo`             |
| Fedora / RHEL   | `wheel`            |

```bash
sudo usermod -aG sudo alice     # Debian/Ubuntu
sudo usermod -aG wheel alice    # Fedora/RHEL
```

Rules live in `/etc/sudoers` — **edit only via**:

```bash
sudo visudo
# or drop-in files:

sudo visudo -f /etc/sudoers.d/alice
```

Never edit `/etc/sudoers` with a normal editor blindly; a syntax error can lock out sudo.

## Example drop-in

```bash
# /etc/sudoers.d/alice  (via visudo -f)

alice ALL=(ALL:ALL) ALL
# passwordless (convenient, less safe):

# alice ALL=(ALL) NOPASSWD: ALL

```

Prefer narrow rules in production (specific commands) over blanket `ALL`.

## sudo vs su vs root login

| Tool                 | Use                                                        |
| -------------------- | ---------------------------------------------------------- |
| `sudo cmd`           | One-shot admin                                             |
| `sudo -i`            | Root shell when you need a session                         |
| `su -`               | Switch user (often to root) using **that** user's password |
| Root GUI/SSH session | Avoid for daily work                                       |

## Related

- [Root](../root/README.md)
- [Users](../users/README.md)
- [Groups](../groups/README.md)
- [Accounts index](../README.md)
