# Networking (Linux)

Tags: `dns` `ip` `linux` `network` `networking`

Host networking basics (interfaces, DNS, firewall). For container networks see [Docker networking](../../containers/docker/networking/README.md).

## Inspect

```bash
ip link
ip addr
ip route
resolvectl status          # systemd-resolved
cat /etc/resolv.conf
ss -ltnp                   # listening TCP ports
```

Older tools (`ifconfig`, `netstat`, `route`) still appear in docs; `ip` / `ss` are the modern defaults.

## Connectivity checks

```bash
ping -c3 1.1.1.1
ping -c3 example.com
curl -I https://example.com
traceroute example.com     # or mtr
```

## Firewall (high level)

| Stack                       | Common on                                      |
| --------------------------- | ---------------------------------------------- |
| **firewalld**               | Fedora / RHEL family                           |
| **UFW**                     | Ubuntu simplicity layer over iptables/nftables |
| **nftables** / **iptables** | Underlying packet filter                       |

Docker manipulates firewall rules for published ports — surprises with UFW/firewalld are common ([Docker on Linux](../../containers/docker/platforms/linux/README.md)).

## DNS

- systemd-resolved: `resolvectl query example.com`
- NetworkManager / systemd-networkd / netplan (Ubuntu) configure interfaces differently by distro.

## Related

- [Sysctl](../linux-kernel/sysctl/README.md)
- [Systemd](../systemd/README.md)
- [Docker networking](../../containers/docker/networking/README.md)
- [Linux index](../README.md)
