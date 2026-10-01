# Dev Containers

Tags: `containers` `dev` `devcontainers` `docker` `oci` `vscode`

**Dev Containers** run your development environment inside a container (toolchain, OS libs, extensions) while you edit from the host IDE. Common with VS Code / Cursor via the Dev Containers spec (`devcontainer.json`).

## Why

- Same Node/Python/Go/tool versions for everyone
- Avoid "works on my machine" host pollution
- Reuse Docker skills ([Dockerfile](../dockerfile/README.md), [Compose](../cli/compose/README.md))

## Minimal layout

```text
project/
  .devcontainer/
    devcontainer.json
    Dockerfile          # optional
  src/
```

```json
{
  "name": "app",
  "image": "mcr.microsoft.com/devcontainers/typescript-node:22",
  "forwardPorts": [3000],
  "postCreateCommand": "npm install"
}
```

Or build from a Dockerfile:

```json
{
  "name": "app",
  "build": { "dockerfile": "Dockerfile" },
  "workspaceFolder": "/workspaces/${localWorkspaceFolderBasename}"
}
```

## Compose-based devcontainers

Point `devcontainer.json` at a Compose file when you need DB/redis sidecars — same ideas as [Compose](../cli/compose/README.md).

## Tips

- Bind the repo into the container (default in most flows); keep heavy deps inside the image when possible.
- On Windows, store the project in **WSL** for performance — [Docker on Windows](../platforms/windows/README.md).
- Don't put secrets in the image; use env / mounts.
- Rebuild the container when the Dockerfile or features change.

## Related

- [Dockerfile](../dockerfile/README.md)
- [Compose](../cli/compose/README.md)
- [CLI index](../cli/README.md)
- [Docker index](../README.md)
