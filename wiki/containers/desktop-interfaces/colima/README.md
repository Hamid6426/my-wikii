# Colima

Tags: `colima` `containers` `desktop` `desktop-interfaces` `linux` `macos` `oci` `vm`

Minimalist **CLI** container runtime for macOS and Linux (Lima-based VM on Mac). Docker-compatible CLI workflow without Docker Desktop's GUI — popular for saving RAM and avoiding Desktop licensing.

```bash
colima start
docker ps    # when docker CLI is pointed at Colima's socket
colima stop
```

Setup details change with versions — follow upstream Colima docs for Docker context integration.

## Related

- [Desktop interfaces index](../README.md)
- [Docker on macOS](../../docker/platforms/macos/README.md)
- [Docker contexts](../../docker/cli/contexts/README.md)
- [OrbStack](../orbstack/README.md)
