# Contributing

This repo is a personal knowledge base of Markdown notes. Keep the structure consistent so people and tools can find things.

## Layout

- All topic notes live under [`wiki/`](wiki/README.md).
- [`wiki/README.md`](wiki/README.md) is the topic index and holds the generated table of contents of every page.
- [`scripts/`](scripts) holds the shell helpers that maintain the wiki.
- Agent instructions and page rules: [`AGENTS.md`](AGENTS.md).

```
learning-and-skills/
├── README.md
├── CONTRIBUTING.md
├── AGENTS.md
├── scripts/
├── langs/
│   ├── html-and-css/  javascript/  typescript/
│   └── csharp/  java/  python/
└── wiki/
    ├── README.md
    ├── linux/
    ├── containers/
    └── ...
```

## Folder-only pages

These rules are mandatory.

1. **Folders classify.** Topics and subtopics are directory names.
2. **Every page is `README.md`.** Do not add `foo.md` or `notes.md`. Exception: language folders (see below).
3. **Nest by meaning, but keep it shallow.** Related notes stay together (for example `containers/docker/platforms/`). Peers on a different layer get sibling folders (Docker, Podman, runtimes). Do not add a parent folder that only holds other folders.
4. **Link the parent.** When you add a folder, link it from the nearest parent `README.md`, and from higher indexes if it is a major section.
5. **Tags.** Every page has a `Tags:` line right under the H1: backticks, space-separated. Folder names are tags. Add a few meaning tags when useful.

After adding or moving pages, run this from the repo root to refresh tags and the table of contents:

```bash
./scripts/sync-tags.sh   # or ./scripts/check.sh to fix and verify everything
```

## Language folders

`html-and-css/`, `javascript/`, `typescript/`, `csharp/`, `java/` and `python/` are courses, not reference trees. They use flat numbered lessons, modeled on `csharp/`:

- `README.md` is the index: a Lessons table linking every lesson in order.
- Lessons are `NN_topic_name.md` (two digits, lowercase, underscores) directly in the language folder. No subfolders for lessons.
- Each lesson starts with `# NN - Title`, then short sections with code blocks, separated by `---` where it helps.
- Runnable example code goes in `examples/` inside the language folder, not inside lessons' folders.
- Link new lessons from the language `README.md` and keep the numbering unbroken.

## Writing style

- Short sections, tables, and command blocks over long prose.
- Keep layers apart (a desktop app is not a container runtime is not an image builder).
- For fast-moving tools, link the official docs and avoid version pins.
- Use relative links. After a move, check that every link still resolves.

## Formatting

`format.sh` cleans Markdown in place. Pass a file or a folder (folders are searched recursively). Run it from the repo root.

```bash
./scripts/format.sh wiki/                     # whole wiki
./scripts/format.sh wiki/<folder>             # one folder
./scripts/format.sh wiki/<folder>/<file>.md   # one file
```

It does the following and nothing else:

- Keeps at most one blank line in a row.
- Puts exactly one space after `#` in headings, after `1.` in numbered lists, and after `>` in quotes.
- Turns `*` and `+` bullets into `-`.
- Trims trailing spaces.
- Aligns tables: pads every cell to the widest cell in its column so the pipes line up, and sizes the `---` row to match. `:---`, `:---:` and `---:` markers are kept.
- Leaves fenced code blocks untouched, tables inside them included.

It does not wrap lines.

### Table rules

- A table row starts and ends with `|`.
- A literal pipe in a cell is written `\|`.
- Width is counted in characters, not bytes, so `©` and `→` count as one.
- Rows with fewer cells than the header are padded with empty cells.

## Git ignore

Never commit `.env`, credentials, or tokens. `graft/` is a local cache and stays ignored.

## Changes

1. Edit only what the change needs. No drive-by rewrites.
2. Update the parent `README.md` tables and lists.
3. Run `./scripts/format.sh` on the files you changed.
4. Keep commits small. The message says why the notes changed.

## Do not

- Invent other filenames for pages.
- Put Linux host notes under `containers/`, or Docker Engine deep-dives under `linux/`, without a clear reason and a cross-link.
- Mix desktop apps, engines, and low-level runtimes in one flat list.
