#!/usr/bin/env python3
"""Repair mojibake in wiki markdown using ftfy."""
from __future__ import annotations

from pathlib import Path

import ftfy

ROOT = Path(__file__).resolve().parents[1] / "wiki"


def main() -> None:
    paths = list(ROOT.rglob("README.md"))

    fixed = 0
    for path in paths:
        text = path.read_text(encoding="utf-8")
        repaired = ftfy.fix_text(text)
        # CP1252 mojibake for ▲ (U+25B2) that ftfy sometimes leaves
        repaired = repaired.replace("â–²", "▲").replace("â\u2013\u00b2", "▲")
        if repaired != text:
            path.write_text(repaired, encoding="utf-8", newline="\n")
            fixed += 1
    print(f"fixed {fixed}")


if __name__ == "__main__":
    main()
