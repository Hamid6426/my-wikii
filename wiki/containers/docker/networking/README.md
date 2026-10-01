# Networking

Tags: `containers` `docker` `network` `networking` `oci`

Docker networks control how containers reach each other, the host, and the internet. Default setups are enough for many apps; custom networks help isolate stacks.

## Common network modes

| Driver / mode        | Behavior                                                         |
| -------------------- | ---------------------------------------------------------------- |
| **bridge** (default) | Private network on the host; NAT to the outside                  |
| **host**             | Container shares host network namespace (Linux; no `-p` mapping) |
| **none**             | No network                                                       |
| **Compose network**  | Project-scoped bridge; DNS by service name                       |

```bash
docker network ls
docker network inspect bridge
docker network create appnet
docker run -d --name api --network appnet myapi:dev
```

## Publishing ports

`-p hostPort:containerPort` maps host → container via the Docker proxy/iptables rules:

```bash
docker run --rm -p 8080:80 nginx:alpine
# http://localhost:8080 on the host

```

| Form                   | Meaning                         |
| ---------------------- | ------------------------------- |
| `-p 8080:80`           | All host interfaces             |
| `-p 127.0.0.1:8080:80` | Host localhost only             |
| `-p 80`                | Random host port → container 80 |

`EXPOSE` in a Dockerfile is documentation; it does **not** publish ports by itself.

## Compose DNS

On the default Compose network, services resolve by name:

```yaml
services:
  web:
    # connects to host "db", port 5432
    environment:
      DATABASE_URL: postgres://app:app@db:5432/app
  db:
    image: postgres:16-alpine
```

No need for container IP addresses in normal Compose apps.

## Compose networking recipes

### `expose` vs `ports`

```yaml
services:
  api:
    expose:
      - "3000" # reachable by other containers on the network only
  web:
    ports:
      - "8080:80" # published on the host
```

`expose` does not publish to the host; `ports` does.

### Multiple networks (frontend / backend)

```yaml
services:
  web:
    networks: [frontend, backend]
  db:
    networks: [backend] # not on frontend — web can reach db; internet clients can't hit db directly via Compose DNS from outside
networks:
  frontend:
  backend:
```

### `extra_hosts`

```yaml
services:
  web:
    extra_hosts:
      - "host.docker.internal:host-gateway" # useful on Linux Engine too
      - "legacy.example:10.0.0.5"
```

### External network

```yaml
networks:
  shared:
    external: true
    name: company_shared_net
```

Attach a Compose project to a pre-created `docker network`.

## Container → host

Patterns vary by platform:

| Platform          | Typical host address from container             |
| ----------------- | ----------------------------------------------- |
| Linux             | Host bridge IP, or `host` network mode          |
| Desktop (Mac/Win) | `host.docker.internal` (convenient special DNS) |

## Debugging

```bash
docker exec -it web sh
# inside: ping db   /   wget -qO- http://api:3000/health

docker port web
docker network inspect <project>_default
```

Firewall note (Linux): Docker manipulates `iptables`/`nftables` for published ports; interactions with UFW/firewalld are a frequent surprise — see [Docker on Linux](../platforms/linux/README.md).

## Related

- [Compose](../cli/compose/README.md)
- [Troubleshooting](../troubleshooting/README.md)
- [Volumes](../volumes/README.md)
- [CLI](../cli/README.md)
- [Security](../security/README.md)
- [Docker index](../README.md)
