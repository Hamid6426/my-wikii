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
./format.sh wiki/                # clean up Markdown (a file or folder)
python scripts/sync-tags.py      # add Tags: lines and rebuild the table of contents in wiki/README.md
python scripts/fix-encoding.py   # repair garbled characters (needs: pip install ftfy)
```

## Contributing

Rules for writing pages are in [`CONTRIBUTING.md`](CONTRIBUTING.md). Agent instructions are in [`AGENTS.md`](AGENTS.md).

## License

[MIT](LICENSE)
