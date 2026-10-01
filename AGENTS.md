# AGENTS.md

Instructions for coding agents working in **learning-and-skills**.

## What this repo is

A personal knowledge base of Markdown notes for learning software engineering. It is not an application. Content lives in `wiki/`. Python scripts in `scripts/` maintain it.

## Layout

| Path                      | What it holds                                                             |
| ------------------------- | ------------------------------------------------------------------------- |
| `wiki/`                   | All topic notes (Linux, containers, C#, Java, and more)                   |
| `wiki/README.md`          | Topic index. The table of contents block is generated.                    |
| `scripts/sync-tags.py`    | Adds `Tags:` lines and rebuilds the table of contents in `wiki/README.md` |
| `scripts/fix-encoding.py` | Repairs garbled characters (needs `pip install ftfy`)                     |

## Wiki rules

Rules for writing pages are here and in `CONTRIBUTING.md`. Read them before editing anything under `wiki/`.

- Folders classify, files do not. Every page is a `README.md` inside its folder, except language folders.
- Language folders (`html`, `css`, `javascript`, `typescript`, `csharp`, `java`, `python`) use flat numbered lessons (`NN_topic_name.md`) with an `# NN - Title` H1 and a `README.md` index with a Lessons table. `csharp/` is the model. Example code goes in `examples/`.
- Never create `something.md` topic files outside language folders (except `README.md` and tooling or config files).
- New notes go in `.../topic-name/README.md`, linked from the parent index.
- Docker lives under `containers/docker/`. Peer tools (Podman, runtimes, builders, desktop GUIs) are siblings under `containers/`. Linux host topics live under `linux/`.
- Every `README.md` has a `Tags:` line under the H1. Folders become tags; extra tags live in `scripts/sync-tags.py` (`EXTRA`).
- Link every new page from its parent `README.md`. Prefer relative links and check the targets exist.
- When renaming or moving folders, update all relative links.

### Style

- Concise. Tables and command examples over prose.
- Separate layers (desktop UX, engine, runtime, builder).
- Say so when nesting is for navigation only and disagrees with history (for example Debian under Ubuntu).
- For fast-moving tools, point at upstream docs and avoid version pins unless useful.

## Commands

Run these from the repo root.

```bash
python scripts/sync-tags.py   # after adding or moving pages
./format.sh wiki/             # format a folder or file (wiki/<folder>, wiki/<folder>/<file>.md)
```

## Do not

- Commit unless asked. Push unless asked.
- Commit `.env`, credentials, or tokens.
- Commit `graft/`. It is a local cache, regenerated with `graft build`.
- Rewrite the whole tree unless asked.
- Make drive-by formatting or refactor changes unrelated to the request.
- Use em dashes in any text.

## Code navigation

The repo is indexed by graft. Use `graft ask "<task>" --source`, `graft grep "<literal>"`, or `graft skeleton <file>` before grepping or reading files. Config is in `.mcp.json` and `.claude/`.
