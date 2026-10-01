# Alpine Linux

Tags: `alpine` `apk` `distro` `distros` `linux` `musl` `others`

- **Role:** Tiny musl-based distro; dominant in containers and appliances.
- **Package manager:** `apk`
- **Init:** OpenRC (not systemd by default)
- **Strengths:** Small images, security-minded defaults, fast to pull in Docker/K8s.
- **Trade-offs:** musl incompatibilities with some glibc-assuming software; not a typical full desktop choice.

```bash
apk update
apk add <package>
```

## Related

- [Others index](../README.md)
- [Docker images](../../../../containers/docker/images/README.md)
- [Package managers](../../../package-managers/README.md)
