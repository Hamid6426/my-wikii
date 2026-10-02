#!/usr/bin/env bash
# Bundle every page in wiki/ and langs/ and every C# example into web/wiki-data.js so web/index.html
# works when opened straight from disk (file://). Re-run after editing notes.
set -euo pipefail
cd "$(dirname "$0")/.."

python3 - <<'PY'
import json, os

pages = {}
for top in ("wiki", "langs"):
    for root, dirs, files in os.walk(top):
        dirs[:] = [d for d in dirs if d != "tmp"]
        for name in files:
            if name.endswith((".md", ".cs")):
                path = os.path.join(root, name)
                with open(path, encoding="utf-8") as f:
                    pages[path.replace(os.sep, "/")] = f.read()

with open("web/wiki-data.js", "w", encoding="utf-8") as out:
    out.write("window.WIKI_DATA = ")
    json.dump(pages, out, ensure_ascii=False, sort_keys=True)
    out.write(";\n")

print(f"web/wiki-data.js: {len(pages)} pages")
PY
