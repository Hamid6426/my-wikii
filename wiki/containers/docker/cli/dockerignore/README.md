# Build context and .dockerignore

Tags: `build` `cli` `containers` `docker` `dockerignore` `oci`

`docker build` sends a **build context** (a directory tree) to the Engine. The Dockerfile only sees files inside that context. A fat context = slow uploads and accidental secret/leak risk.

## What gets sent

```bash
docker build -t myapp:dev .
#                         ^ context directory (often ".")

```

Everything under the context path is considered unless excluded by **`.dockerignore`** (same idea as `.gitignore`).

## Minimal `.dockerignore`

```gitignore
.git
.gitignore
Dockerfile*
.dockerignore
node_modules
**/__pycache__
*.pyc
.env
.env.*
dist
build
coverage
*.md
.vscode
.idea
```

Tune per language. Never rely on "I didn't COPY .env" alone — secrets in the context can still appear in unused layers or cache metadata depending on usage.

## Common mistakes

| Mistake                                                | Fix                                                  |
| ------------------------------------------------------ | ---------------------------------------------------- |
| Context is repo root with huge `node_modules` / `.git` | Add `.dockerignore`; or use a smaller context dir    |
| `COPY . .` early                                       | Copy dependency manifests first; ignore junk         |
| Building from `C:\` / huge bind on Desktop             | Use a tight project folder; on Windows prefer WSL fs |
| Secrets in context                                     | `.dockerignore` + BuildKit `--secret` / runtime env  |

## Check what's included (rough)

There isn't one perfect "list context" flag everywhere; practical checks:

- Keep `.dockerignore` next to the Dockerfile you build.
- Watch build start time / "transferring context" messages.
- Ensure CI checkout isn't syncing build artifacts into the context.

## Related

- [Dockerfile](../../dockerfile/README.md)
- [Build / Buildx](../build/README.md)
- [Security](../../security/README.md)
- [CLI index](../README.md)
