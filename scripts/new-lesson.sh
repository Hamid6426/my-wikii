#!/usr/bin/env bash
# Create the next numbered lesson in a language folder and add it to the Lessons table.
# Usage: ./scripts/new-lesson.sh <lang> "<Title>"
#   lang:  html-and-css javascript typescript csharp java python
#          (also accepted as langs/<lang> or wiki/langs/<lang>)
#   Title: the lesson title, used for the H1, the file name and the table row
#   WIKI_ROOT: override the wiki folder (default: <repo>/wiki). When set, sync-tags.sh is not run.
# Examples: ./scripts/new-lesson.sh csharp "Span and Memory"
#           ./scripts/new-lesson.sh wiki/langs/python "List Comprehensions"
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WIKI="${WIKI_ROOT:-$ROOT/wiki}"
README_NAME="README.md"
LANGS=(html-and-css javascript typescript csharp java python)

die() {
  echo "new-lesson: $*" >&2
  exit 1
}

if (($# != 2)); then
  echo "Usage: $0 <lang> \"<Title>\"" >&2
  exit 1
fi

lang="$1"
title="$2"

# Normalise the language: drop a leading ./, wiki/ and langs/, and trailing slashes.
lang="${lang#./}"
lang="${lang#wiki/}"
lang="${lang#langs/}"
while [[ "$lang" == */ ]]; do lang="${lang%/}"; done

known=0
for l in "${LANGS[@]}"; do
  [[ "$lang" == "$l" ]] && known=1
done
((known)) || die "unknown language '$1'. Use one of: ${LANGS[*]}"

dir="$WIKI/langs/$lang"
readme="$dir/$README_NAME"
[[ -d "$dir" ]] || die "language folder not found: wiki/langs/$lang"
[[ -f "$readme" ]] || die "language index not found: wiki/langs/$lang/$README_NAME"

title="${title#"${title%%[![:space:]]*}"}"
title="${title%"${title##*[![:space:]]}"}"
[[ -n "$title" ]] || die "title is empty"
[[ "$title" != *$'\n'* ]] || die "title must be one line"
[[ "$title" != *"|"* ]] || die "title must not contain '|' (it breaks the Lessons table)"

slug="$(tr '[:upper:]' '[:lower:]' <<<"$title" | sed -E 's/[^a-z0-9]+/_/g; s/^_+//; s/_+$//')"
[[ -n "$slug" ]] || die "title has no letters or digits to build a file name from"

# Highest NN among NN_*.md lessons; none means the first lesson is 01.
max=0
shopt -s nullglob
for f in "$dir"/[0-9][0-9]_*.md; do
  n="${f##*/}"
  n=$((10#${n:0:2}))
  ((n > max)) && max=$n
done
existing=("$dir"/[0-9][0-9]_"$slug".md)
shopt -u nullglob

((${#existing[@]} == 0)) || die "a lesson with this name already exists: wiki/langs/$lang/${existing[0]##*/}"
((max < 99)) || die "lesson numbers are two digits and 99 is taken"

nn="$(printf '%02d' $((max + 1)))"
file="${nn}_${slug}.md"
lesson="$dir/$file"
[[ ! -e "$lesson" ]] || die "already exists, not overwriting: wiki/langs/$lang/$file"

# Build the new README text. The row goes after the last row of the table under
# `## Lessons`, padded to the widths of the separator row when the text fits.
# With no table there, one is added under the heading (or a new `## Lessons` at the end).
tmp="$(mktemp)"
trap 'rm -f "$tmp"' EXIT

awk -v nn="$nn" -v cell="[$title]($file)" '
  function pad(s, w) { while (length(s) < w) s = s " "; return s }
  { lines[++n] = $0 }
  END {
    heading = 0; first = 0; last = 0
    for (i = 1; i <= n; i++) if (lines[i] ~ /^## Lessons[ \t]*$/) { heading = i; break }
    if (heading) {
      for (i = heading + 1; i <= n && lines[i] !~ /^#{1,2} /; i++) {
        if (lines[i] ~ /^\|/) {
          if (!first) first = i
          last = i
        } else if (first) break
      }
    }

    if (last) {
      # Column widths from the separator row (the second table line).
      sep = lines[first + 1]
      cols = split(sep, c, "|") - 2
      for (k = 1; k <= cols; k++) { gsub(/^ +| +$/, "", c[k + 1]); w[k] = length(c[k + 1]) }
      row = "| " pad(nn, w[1]) " | " pad(cell, w[2]) " |"
      for (k = 3; k <= cols; k++) row = row " " pad("", w[k]) " |"
      for (i = 1; i <= n; i++) { print lines[i]; if (i == last) print row }
      exit
    }

    table[1] = "| No. | Lesson |"
    table[2] = "| --- | --- |"
    table[3] = "| " pad(nn, 3) " | " pad(cell, 3) " |"

    if (heading) {
      for (i = 1; i <= heading; i++) print lines[i]
      print ""
      for (k = 1; k <= 3; k++) print table[k]
      i = heading + 1
      while (i <= n && lines[i] == "") i++
      if (i <= n) print ""
      for (; i <= n; i++) print lines[i]
      exit
    }

    while (n > 0 && lines[n] == "") n--
    for (i = 1; i <= n; i++) print lines[i]
    if (n > 0) print ""
    print "## Lessons"
    print ""
    for (k = 1; k <= 3; k++) print table[k]
  }
' "$readme" >"$tmp"

{
  printf '# %s - %s\n\n' "$nn" "$title"
  printf '## Overview\n\n'
  printf 'Notes on %s.\n' "$title"
} >"$lesson"
# Overwrite in place so file permissions are kept.
cat "$tmp" >"$readme"

echo "Created: wiki/langs/$lang/$file"
echo "Edited:  wiki/langs/$lang/$README_NAME (added lesson $nn to the Lessons table)"

if [[ -n "${WIKI_ROOT:-}" ]]; then
  echo "WIKI_ROOT is set, skipped scripts/sync-tags.sh"
else
  "$ROOT/scripts/sync-tags.sh"
fi
