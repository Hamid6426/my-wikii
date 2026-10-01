#!/usr/bin/env bash
# Bundle every wiki page into web/wiki-data.js so web/index.html
# works when opened straight from disk (file://). Re-run after editing notes.
set -euo pipefail
cd "$(dirname "$0")/.."

python3 - <<'PY'
import json, os

pages = {}
for root, dirs, files in os.walk("wiki"):
    dirs[:] = [d for d in dirs if d != "tmp"]
    for name in files:
        if name.endswith(".md"):
            path = os.path.join(root, name)
            with open(path, encoding="utf-8") as f:
                pages[os.path.relpath(path, "wiki").replace(os.sep, "/")] = f.read()

with open("web/wiki-data.js", "w", encoding="utf-8") as out:
    out.write("window.WIKI_DATA = ")
    json.dump(pages, out, ensure_ascii=False, sort_keys=True)
    out.write(";\n")

print(f"web/wiki-data.js: {len(pages)} pages")
PY
