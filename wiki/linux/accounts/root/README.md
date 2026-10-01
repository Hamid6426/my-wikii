# Root

Tags: `accounts` `identity` `linux` `root` `security` `uid`

**Root** is the superuser account (**UID 0**). It can read/write almost any file, bind privileged ports, load modules, and manage all users.

## Check

```bash
id
# uid=0(root) gid=0(root) …  → you are root

sudo -i          # root login shell via sudo
su -             # switch to root (if allowed / password known)
```

## When to use it

| Do                                                  | Don't                                       |
| --------------------------------------------------- | ------------------------------------------- |
| Package installs, system config, service management | Daily browsing, coding, email as root       |
| Break-glass recovery                                | Leave a blank/remote root password exposed  |
| `sudo` for one command                              | Habitual `sudo su` for hours of normal work |

Prefer **[sudo](../sudo/README.md)** for admin tasks from a normal user account.

## Root login

Distros differ:

- Many cloud images **disable password root SSH** and use keys + sudo.
- Desktops often lock the root account and rely on sudo (Ubuntu-style).
- Some setups allow `su -` with a root password (classic).

```bash
sudo passwd root     # set/unlock root password (policy-dependent)
sudo passwd -l root  # lock root password (common hardening)
```

## Files owned by root

System paths (`/etc`, `/usr`, many `/var` trees) are root-owned. Editing them needs sudo. Your home should be **your** UID — if you create files in `$HOME` as root, fix with `chown`:

```bash
sudo chown -R "$USER:$USER" ~/some-dir
```

## Related

- [Sudo](../sudo/README.md)
- [Users](../users/README.md)
- [Accounts index](../README.md)
