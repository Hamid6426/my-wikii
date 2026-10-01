# Podman Compose

Tags: `compose` `containers` `oci` `orchestration` `podman`

Compose-style multi-container runs with Podman:

```bash
podman compose up -d
podman compose ps
podman compose down
```

Implementation has shifted over time (`podman-compose` Python tool vs integrated `podman compose`). Check `podman compose version` / distro packages.

YAML is largely the same Compose spec family as [Docker Compose](../../docker/cli/compose/README.md); test `depends_on`, build, and network edge cases when migrating.

## Related

- [Podman index](../README.md)
- [Docker Compose](../../docker/cli/compose/README.md)
- [Docker networking recipes](../../docker/networking/README.md)
