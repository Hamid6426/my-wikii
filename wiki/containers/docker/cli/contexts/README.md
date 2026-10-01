# Contexts / remote Docker

Tags: `cli` `containers` `contexts` `docker` `oci` `remote`

A **Docker context** points the CLI at a particular Engine (local Desktop, SSH host, TCP endpoint). Useful for deploying to a remote daemon or switching between machines.

## List and use

```bash
docker context ls
docker context show
docker context use default
```

Desktop often creates contexts like `desktop-linux`. The active context determines where `docker ps` runs.

## Create (SSH example)

```bash
docker context create myvm --docker "host=ssh://user@myvm.example"
docker context use myvm
docker ps
```

Requires SSH access and a Docker Engine on the remote host that your user can reach (rootless or group membership as configured there).

## `DOCKER_HOST`

Older/alternate approach:

```bash
export DOCKER_HOST=ssh://user@myvm.example
# or tcp://… with TLS carefully configured

docker info
```

Prefer **contexts** for day-to-day switching; they compose better with Desktop.

## Safety

| Risk               | Note                                                                               |
| ------------------ | ---------------------------------------------------------------------------------- |
| Remote root Engine | Same as local socket — full host control                                           |
| TCP without TLS    | Don't expose `dockerd` on the public internet raw                                  |
| Wrong context      | `docker compose down` against production by mistake — always `docker context show` |

## CI pattern

CI runners either:

- Talk to a local Engine/sidecar, or
- Use cloud builders / remote contexts / registry-only workflows (`buildx --push`)

## Related

- [CLI index](../README.md)
- [Security](../../security/README.md)
- [Build / Buildx](../build/README.md)
- [Platforms](../../platforms/README.md)
