# Groups

Tags: `accounts` `gid` `groups` `identity` `linux`

**Groups** grant shared access. Every user has one **primary group** (in `/etc/passwd`) and may have **supplementary** groups (in `/etc/group`).

## Inspect

```bash
id
groups
getent group
getent group docker
```

File access uses owner UID + group GID + other (`ls -l` → `rwx` triplets).

## Create and membership

```bash
sudo groupadd deploy
sudo usermod -aG deploy alice    # -aG append supplementary (don't omit -a)
sudo gpasswd -d alice deploy     # remove from group
```

**Important:** `-aG` appends. `usermod -G` **without** `-a` replaces the whole supplementary list.

New group membership often needs a **new login** (or `newgrp`) before it applies to your session.

```bash
newgrp deploy
# or log out/in

id
```

## Common groups

| Group                     | Typical meaning                                        |
| ------------------------- | ------------------------------------------------------ |
| `sudo` / `wheel`          | May use sudo (distro-dependent name)                   |
| `docker`                  | Access Docker socket — **effectively root-equivalent** |
| `adm` / `systemd-journal` | Read logs (varies)                                     |
| `video` / `render`        | GPU/access devices                                     |
| Primary group = username  | Private group per user (common on Debian/Ubuntu)       |

## Related

- [Users](../users/README.md)
- [Sudo](../sudo/README.md)
- [Docker security](../../../containers/docker/security/README.md)
- [Accounts index](../README.md)
