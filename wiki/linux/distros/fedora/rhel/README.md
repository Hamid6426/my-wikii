# RHEL

Tags: `distro` `distros` `enterprise` `fedora` `linux` `rhel` `rpm`

Red Hat Enterprise Linux and RHEL-compatible rebuilds. Shares the `.rpm` / DNF ecosystem with [Fedora](../README.md) upstream.

## RHEL (Red Hat Enterprise Linux)

- **Role:** Paid enterprise OS with long support, certifications, and support contracts.
- **Strengths:** Stability, security errata, lifecycle clarity, ecosystem (Satellite, Ansible, OpenShift adjacency).
- **Trade-offs:** Subscription model; packages intentionally conservative vs Fedora.

## CentOS Stream & rebuilds

| Distro            | Relationship                           |
| ----------------- | -------------------------------------- |
| **CentOS Stream** | Rolling preview slightly ahead of RHEL |
| **Rocky Linux**   | Community RHEL-compatible rebuild      |
| **AlmaLinux**     | Community RHEL-compatible rebuild      |
| **Oracle Linux**  | RHEL-compatible with Oracle's extras   |

For "CentOS-like" free servers after classic CentOS Linux ended: **Rocky** or **AlmaLinux** are the usual answers.

```bash
# RHEL-compatible systems

sudo dnf update
cat /etc/os-release
```
