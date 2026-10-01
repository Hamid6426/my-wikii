# containerd

Tags: `containerd` `containers` `cri` `oci` `runtime` `runtimes`

**containerd** is a daemon that manages image transfer/storage and container lifecycle. Docker Engine speaks to containerd; many Kubernetes clusters use containerd directly via CRI.

```bash
# when ctr/nerdctl available

ctr version
nerdctl run --rm nginx:alpine
```

**nerdctl** is a Docker-like CLI for containerd. You rarely replace "Docker the product" with raw `ctr` for app dev — you replace the _engine UX_ while keeping OCI images.

## Related

- [Runtimes index](../README.md)
- [CRI-O](../cri-o/README.md)
- [Docker](../../docker/README.md)
- [Kaniko](../../image-builders/kaniko/README.md) — builds in-cluster without Docker socket
