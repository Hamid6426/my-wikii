# Users

Tags: `accounts` `identity` `linux` `passwd` `uid` `users`

A **user account** maps a name → **UID**, primary **GID**, home directory, and login shell.

## Inspect

```bash
getent passwd
getent passwd alice
ls -la /home
echo $HOME $SHELL
```

`/etc/passwd` fields (colon-separated):
`name:x:UID:GID:gecos:home:shell`

## Create / remove

Debian/Ubuntu-friendly (interactive defaults):

```bash
sudo adduser alice
sudo deluser alice          # may keep home depending on flags
sudo deluser --remove-home alice
```

Lower-level (Fedora/RHEL/Arch and scripting):

```bash
sudo useradd -m -s /bin/bash alice
sudo passwd alice
sudo userdel -r alice        # -r removes home
```

| Flag (useradd)     | Meaning              |
| ------------------ | -------------------- |
| `-m`               | Create home          |
| `-s /bin/bash`     | Login shell          |
| `-G group1,group2` | Supplementary groups |
| `-u 1500`          | Force UID (careful)  |

## Passwords

```bash
passwd                 # change your own
sudo passwd alice      # set another user's password
sudo passwd -l alice   # lock
sudo passwd -u alice   # unlock
```

Hashes live in `/etc/shadow`, not `/etc/passwd`.

## Shell and home

```bash
chsh -s /bin/zsh
getent passwd $USER
sudo usermod -d /new/home -m alice   # move home (advanced; know what you're doing)
```

Service accounts often use `/usr/sbin/nologin` or `/bin/false` as the shell so nobody logs in as that user.

## Related

- [Groups](../groups/README.md)
- [Sudo](../sudo/README.md)
- [Root](../root/README.md)
- [Shells](../../shells-and-terminals/shells/README.md)
- [Accounts index](../README.md)
