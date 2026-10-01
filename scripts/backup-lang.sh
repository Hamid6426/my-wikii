#!/usr/bin/env bash
# Copy a language folder into wiki/langs/<lang>/tmp/ so a risky change can be undone.
# Usage: ./scripts/backup-lang.sh <lang>
#   lang: html-and-css javascript typescript csharp java python
#         (also accepted as langs/<lang> or wiki/langs/<lang>)
# Replaces any existing backup. The backup is git-ignored and skipped by every script.
# Restore from wiki/langs/<lang> with:
#   find . -mindepth 1 -maxdepth 1 ! -name tmp -exec rm -rf {} + && cp -a tmp/. .
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WIKI="${WIKI_ROOT:-$ROOT/wiki}"
LANGS=(html-and-css javascript typescript csharp java python)

die() {
  echo "backup-lang: $*" >&2
  exit 1
}

if (($# != 1)); then
  echo "Usage: $0 <lang>" >&2
  echo "  lang: ${LANGS[*]}" >&2
  exit 1
fi

lang="$1"
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
[[ -d "$dir" ]] || die "language folder not found: wiki/langs/$lang"
[[ -f "$dir/README.md" ]] || die "language index not found: wiki/langs/$lang/README.md"

backup="$dir/tmp"
rm -rf "$backup"
mkdir -p "$backup"
# Copy every entry except the backup folder itself.
find "$dir" -mindepth 1 -maxdepth 1 ! -name tmp -exec cp -a {} "$backup/" \;

count="$(find "$backup" -type f | wc -l)"
echo "Backed up: wiki/langs/$lang/ -> wiki/langs/$lang/tmp/ ($count files)"
echo "Restore:   cd wiki/langs/$lang && find . -mindepth 1 -maxdepth 1 ! -name tmp -exec rm -rf {} + && cp -a tmp/. ."
