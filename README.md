# Learning and Skills

Personal notes and reference material for learning software engineering. Everything lives in the [wiki](wiki/README.md).

## Layout

| Path                      | What it holds                                      |
| ------------------------- | -------------------------------------------------- |
| [`wiki/`](wiki/README.md) | Topic notes: Linux, containers, C#, Java, and more |
| [`scripts/`](scripts)     | Python helpers that maintain the wiki              |
| [`format.sh`](format.sh)  | Markdown cleanup for a file or folder              |

## Scripts

Run these from the repo root.

```bash
./scripts/check.sh            # run every fix and check (add --no-fix to only check)
./scripts/sync-tags.sh       # add Tags: lines and rebuild the table of contents in wiki/README.md
./scripts/format.sh wiki/    # format Markdown (a file or folder)
./scripts/fix-encoding.sh    # repair garbled characters
./scripts/check-links.sh     # find broken relative links
./scripts/check-structure.sh # enforce the wiki layout rules
./scripts/check-dashes.sh    # find em dashes
./scripts/check-secrets.sh   # find .env files and secret-looking strings
./scripts/check-examples.sh  # compile the example code
./scripts/stats.sh           # page counts and thin pages
./scripts/new-page.sh <path> "<Title>"   # new topic page, linked from its parent
./scripts/new-lesson.sh <lang> "<Title>" # next numbered lesson in a language folder
```

## Contributing

Rules for writing pages are in [`CONTRIBUTING.md`](CONTRIBUTING.md). Agent instructions are in [`AGENTS.md`](AGENTS.md).

## License

[MIT](LICENSE)
