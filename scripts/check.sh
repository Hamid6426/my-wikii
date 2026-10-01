#!/usr/bin/env bash
# Run every repo check and fix step in order. Stops at the end with a summary.
# Usage: ./scripts/check.sh [--no-fix] [path...]
#   path: a folder or file to limit every step to (wiki/, wiki/linux, wiki/linux/README.md). Default: whole repo.
#   --no-fix: skip the steps that rewrite files (fix-encoding, format, sync-tags)
set -uo pipefail

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

fix=1
if [[ "${1:-}" == "--no-fix" ]]; then
  fix=0
  shift
fi
scope=("$@")
wiki_scope=("${scope[@]}")
((${#wiki_scope[@]} == 0)) && wiki_scope=(wiki/)

failed=()

run() {
  local name="$1"
  shift
  echo "== $name"
  if "$@"; then
    echo "-- $name: ok"
  else
    echo "-- $name: FAILED"
    failed+=("$name")
  fi
  echo
}

if ((fix)); then
  run fix-encoding ./scripts/fix-encoding.sh "${wiki_scope[@]}"
  run format ./scripts/format.sh "${wiki_scope[@]}"
  run sync-tags ./scripts/sync-tags.sh "${wiki_scope[@]}"
fi

run check-links ./scripts/check-links.sh "${scope[@]}"
run check-structure ./scripts/check-structure.sh "${wiki_scope[@]}"
run check-dashes ./scripts/check-dashes.sh "${scope[@]}"
run check-secrets ./scripts/check-secrets.sh "${scope[@]}"
run check-examples ./scripts/check-examples.sh "${wiki_scope[@]}"

if ((${#failed[@]} == 0)); then
  echo "all checks passed"
else
  echo "failed: ${failed[*]}"
  exit 1
fi
