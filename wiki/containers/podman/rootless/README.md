# Podman rootless

Tags: `containers` `oci` `podman` `rootless` `security`

Rootless Podman runs containers inside **user namespaces** — compromise of a container is less likely to mean host root.

## Basics

```bash
podman info | grep -i rootless
podman run --rm quay.io/podman/hello
```

On some distros you may need `uidmap`/`gidmap` (`shadow-utils`, `/etc/subuid`, `/etc/subgid`) configured for your user.

## Common limits

| Topic        | Note                                                           |
| ------------ | -------------------------------------------------------------- |
| Ports < 1024 | Often need config or higher host ports                         |
| Linger       | `loginctl enable-linger $USER` so user services survive logout |
| Storage      | User-scoped storage under home; disk quotas apply              |
| Cgroups      | cgroup v2 hosts work best                                      |

## systemd / Quadlet

Rootless services can be managed with systemd user units or **Quadlet** (`.container` files) on modern Podman — see distro docs.

## Related

- [Podman index](../README.md)
- [Docker security / rootless Engine](../../docker/security/README.md)
- [Systemd](../../../linux/systemd/README.md)
