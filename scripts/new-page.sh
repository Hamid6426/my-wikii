#!/usr/bin/env bash
# Create a new wiki page (<path>/README.md) and link it from its parent README.md.
# Usage: ./scripts/new-page.sh <path-under-wiki> "<Title>"
#   path:  folder path under wiki/, lowercase letters, digits and hyphens per segment
#   Title: the H1 of the new page and the link text in the parent
#   WIKI_ROOT: override the wiki folder (default: <repo>/wiki). When set, sync-tags.sh is not run.
# Examples: ./scripts/new-page.sh linux/networking/dns "DNS"
#           ./scripts/new-page.sh containers/podman/quadlet "Quadlet"
# Lessons in language folders (wiki/langs/<lang>/) use ./scripts/new-lesson.sh instead.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WIKI="${WIKI_ROOT:-$ROOT/wiki}"
README_NAME="README.md"
TOC_START="<!-- toc:start -->"
TOC_END="<!-- toc:end -->"

die() {
  echo "new-page: $*" >&2
  exit 1
}

if (($# != 2)); then
  echo "Usage: $0 <path-under-wiki> \"<Title>\"" >&2
  exit 1
fi

rel="$1"
title="$2"

[[ -d "$WIKI" ]] || die "wiki folder not found: $WIKI"

# Normalise the path: drop a leading ./ or wiki/, and trailing slashes.
rel="${rel#./}"
rel="${rel#wiki/}"
while [[ "$rel" == */ ]]; do rel="${rel%/}"; done

[[ -n "$rel" ]] || die "path is empty"
[[ "$rel" != /* ]] || die "path must be relative to wiki/: $1"

IFS=/ read -r -a parts <<<"$rel"
for part in "${parts[@]}"; do
  [[ "$part" =~ ^[a-z0-9][a-z0-9-]*$ ]] ||
    die "bad path segment '$part': use lowercase letters, digits and hyphens"
done

if [[ "${parts[0]}" == "langs" ]] && ((${#parts[@]} >= 2)); then
  die "$rel is inside a language folder. Use scripts/new-lesson.sh for lessons."
fi

title="${title#"${title%%[![:space:]]*}"}"
title="${title%"${title##*[![:space:]]}"}"
[[ -n "$title" ]] || die "title is empty"
[[ "$title" != *$'\n'* ]] || die "title must be one line"

name="${parts[${#parts[@]} - 1]}"
if ((${#parts[@]} == 1)); then
  parent_dir="$WIKI"
  parent_rel="$README_NAME"
else
  parent_dir="$WIKI/${rel%/*}"
  parent_rel="${rel%/*}/$README_NAME"
fi
parent="$parent_dir/$README_NAME"
page_dir="$WIKI/$rel"
page="$page_dir/$README_NAME"

[[ -f "$parent" ]] || die "parent page not found: wiki/$parent_rel (create it first)"
[[ ! -e "$page" ]] || die "already exists, not overwriting: wiki/$rel/$README_NAME"

link="- [$title]($name/$README_NAME)"

# Build the new parent text. Where the link goes, in order of preference:
#   1. after the last bullet that links a child page (`](child/README.md)`)
#   2. at the end of an existing `## Pages` section (its "_No pages yet._" line is removed)
#   3. under a new `## Pages` heading at the end of the file
# Lines inside the generated table of contents are ignored.
tmp="$(mktemp)"
trap 'rm -f "$tmp"' EXIT

awk -v link="$link" -v toc_start="$TOC_START" -v toc_end="$TOC_END" '
  { lines[++n] = $0 }
  END {
    in_toc = 0; last_child = 0; pages = 0; pages_end = 0; in_pages = 0
    for (i = 1; i <= n; i++) {
      if (lines[i] == toc_start) { in_toc = 1; continue }
      if (lines[i] == toc_end) { in_toc = 0; continue }
      if (in_toc) continue
      if (lines[i] ~ /^## /) {
        in_pages = (lines[i] ~ /^## Pages[ \t]*$/)
        if (in_pages && !pages) pages = i
      }
      if (lines[i] ~ /^- \[[^]]*\]\([A-Za-z0-9][^):]*\/README\.md\)/) last_child = i
      if (in_pages && lines[i] != "") pages_end = i
    }

    if (last_child) {
      for (i = 1; i <= n; i++) {
        print lines[i]
        if (i == last_child) print link
      }
      exit
    }

    if (pages) {
      for (i = 1; i <= n; i++) {
        if (i > pages && i <= pages_end && lines[i] ~ /^_No pages yet\._[ \t]*$/) {
          if (i == pages_end) print link
          continue
        }
        print lines[i]
        if (i == pages_end) {
          if (i == pages) print ""
          print link
        }
      }
      exit
    }

    # Drop trailing blank lines, then add the section.
    while (n > 0 && lines[n] == "") n--
    for (i = 1; i <= n; i++) print lines[i]
    if (n > 0) print ""
    print "## Pages"
    print ""
    print link
  }
' "$parent" >"$tmp"

mkdir -p "$page_dir"
{
  printf '# %s\n\n' "$title"
  printf 'Tags:\n\n'
  printf 'Notes on %s.\n' "$title"
} >"$page"
# Overwrite in place so file permissions are kept.
cat "$tmp" >"$parent"

echo "Created: wiki/$rel/$README_NAME"
echo "Edited:  wiki/$parent_rel (added $link)"

if [[ -n "${WIKI_ROOT:-}" ]]; then
  echo "WIKI_ROOT is set, skipped scripts/sync-tags.sh"
else
  "$ROOT/scripts/sync-tags.sh"
fi
