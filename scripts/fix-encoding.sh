#!/usr/bin/env bash
# Repair mojibake in wiki markdown (UTF-8 text that was decoded as CP1252 and saved again).
# Usage: ./scripts/fix-encoding.sh [path...]
#   path: a README.md file or a folder (all README.md files under it). Default: wiki/
# Examples: ./scripts/fix-encoding.sh   ./scripts/fix-encoding.sh wiki/linux   ./scripts/fix-encoding.sh wiki/linux/README.md
# Works line by line: a line is repaired only if re-encoding it as CP1252 succeeds
# and the result is valid UTF-8. Clean lines are left alone.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)/wiki"

command -v iconv >/dev/null || { echo "iconv is required" >&2; exit 1; }

# fix_stream: stdin to stdout, repairing each line that looks like mojibake.
fix_stream() {
  local line repaired
  while IFS= read -r line || [[ -n "$line" ]]; do
    # Pure ASCII needs no repair.
    if [[ "$line" != *[$'\x80'-$'\xff']* ]]; then
      printf '%s\n' "$line"
      continue
    fi
    if repaired="$(printf '%s' "$line" | iconv -f UTF-8 -t CP1252 2>/dev/null | iconv -f UTF-8 -t UTF-8 2>/dev/null)" && [[ -n "$repaired" ]]; then
      printf '%s\n' "$repaired"
    else
      printf '%s\n' "$line"
    fi
  done
}

fix_file() {
  local path="$1" tmp
  tmp="$(mktemp)"
  fix_stream <"$path" >"$tmp"
  # Mojibake for the triangle (U+25B2) that the generic repair can miss.
  sed -i -e $'s/\xc3\xa2\xe2\x80\x93\xc2\xb2/\xe2\x96\xb2/g' "$tmp"
  if ! cmp -s "$path" "$tmp"; then
    cat "$tmp" >"$path"
    fixed=$((fixed + 1))
  fi
  rm -f "$tmp"
}

fixed=0
status=0
(($# == 0)) && set -- "$ROOT"
for target in "$@"; do
  if [[ -d "$target" ]]; then
    while IFS= read -r -d '' f; do
      fix_file "$f"
    done < <(find "$target" -type f -name 'README.md' -print0 | sort -z)
  elif [[ -f "$target" ]]; then
    fix_file "$target"
  else
    echo "Not found: $target" >&2
    status=1
  fi
done

echo "fixed $fixed"
exit $status
