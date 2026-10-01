# Accounts

Tags: `accounts` `identity` `linux` `security`

Linux identity: **users**, **groups**, and **root**. Permissions, files, and processes all key off numeric **UID** / **GID**.

## Pages

| Page                       | Focus                                        |
| -------------------------- | -------------------------------------------- |
| [Root](root/README.md)     | UID 0, when to use it, risks                 |
| [Users](users/README.md)   | adduser/useradd, passwd, home, shells        |
| [Groups](groups/README.md) | Primary vs supplementary groups              |
| [Sudo](sudo/README.md)     | Least-privilege admin without living as root |

## Who am I?

```bash
whoami
id
id username
getent passwd $USER
getent group sudo     # or wheel on Fedora/RHEL
```

| File                            | Role                                                    |
| ------------------------------- | ------------------------------------------------------- |
| `/etc/passwd`                   | Username, UID, GID, home, shell (not the password hash) |
| `/etc/shadow`                   | Password hashes (root-readable)                         |
| `/etc/group`                    | Group names and members                                 |
| `/etc/sudoers` (+ `sudoers.d/`) | Who may run what as root via sudo                       |

## Mental model

```
user (UID) ──belongs to──► groups (GIDs)
                │
                └── owns files / runs processes
```

**Root** is just the special account with UID **0** — not "a different OS," the same permission system with all caps unlocked.

## Related

- [Shells](../shells-and-terminals/shells/README.md) — login shell (`chsh`)
- [Resources / limits](../resources/limits/README.md) — per-user process limits
- [Podman rootless](../../containers/podman/rootless/README.md)
- [Docker security](../../containers/docker/security/README.md) — `docker` group ≈ root
- [Linux index](../README.md)
