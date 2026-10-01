#!/usr/bin/env bash
# Report relative Markdown links whose target file or folder does not exist.
# Usage: ./scripts/check-links.sh [path...]
#   path: a .md file or a folder (default: the whole repo)
# Skips web links, mailto, #anchors, code blocks and inline code.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

# Find a path given relative to the cwd, relative to the repo root, or absolute
resolve_target() {
  if [[ -e "$1" ]]; then
    realpath "$1"
  elif [[ "$1" != /* && -e "$ROOT/$1" ]]; then
    realpath "$ROOT/$1"
  else
    return 1
  fi
}

# Show paths inside the repo relative to its root, others as absolute paths
show_path() {
  case "$1" in
    "$ROOT"/*) echo "${1#"$ROOT"/}" ;;
    *) echo "$1" ;;
  esac
}

# Print "line<TAB>target" for every link outside code in one Markdown file
extract_links() {
  awk '
    {
      line = $0
      stripped = line
      sub(/^[ \t]+/, "", stripped)
      # Fenced code blocks: close only on the same marker
      if (stripped ~ /^(```|~~~)/) {
        marker = substr(stripped, 1, 3)
        if (fence == "") fence = marker
        else if (fence == marker) fence = ""
        next
      }
      if (fence != "") next
      # Drop inline code spans
      gsub(/`[^`]*`/, "", line)
      while (match(line, /\[[^]]*\]\([^)]+\)/)) {
        link = substr(line, RSTART, RLENGTH)
        line = substr(line, RSTART + RLENGTH)
        sub(/^\[[^]]*\]\(/, "", link)
        sub(/\)$/, "", link)
        sub(/^[ \t]+/, "", link)
        # Angle bracket form <path with spaces>, else drop an optional "title"
        if (link ~ /^</) {
          sub(/^</, "", link)
          sub(/>.*$/, "", link)
        } else {
          sub(/[ \t].*$/, "", link)
        }
        print NR "\t" link
      }
    }
  ' "$1"
}

# Decode %XX escapes such as %20
url_decode() {
  local s="${1//+/%2B}"
  printf '%b' "${s//%/\\x}"
}

problems=0

check_file() {
  local file="$1" rel dir lineno target path
  rel="$(show_path "$file")"
  dir="$(dirname "$file")"
  while IFS=$'\t' read -r lineno target; do
    case "$target" in
      http://* | https://* | mailto:* | '#'* | '') continue ;;
    esac
    path="${target%%#*}"
    path="${path%%\?*}"
    [[ -z "$path" ]] && continue
    path="$(url_decode "$path")"
    if [[ "$path" == /* ]]; then
      path="$ROOT$path"
    else
      path="$dir/$path"
    fi
    if [[ ! -e "$path" ]]; then
      echo "$rel:$lineno: broken link: $target"
      problems=$((problems + 1))
    fi
  done < <(extract_links "$file")
}

if [[ $# -eq 0 ]]; then
  set -- "$ROOT"
fi

status=0
targets=()
for target in "$@"; do
  if resolved="$(resolve_target "$target")"; then
    targets+=("$resolved")
  else
    echo "Not found: $target" >&2
    status=1
  fi
done

if ((${#targets[@]} == 0)); then
  exit "$status"
fi

for target in "${targets[@]}"; do
  if [[ -d "$target" ]]; then
    while IFS= read -r -d '' f; do
      check_file "$f"
    done < <(find "$target" \( -name .git -o -name graft -o -name node_modules -o -name tmp \) -prune \
      -o -type f -name '*.md' -print0 | sort -z)
  else
    check_file "$target"
  fi
done

if ((problems > 0)); then
  echo "$problems problem(s)"
  exit 1
fi
echo "ok: no broken links"
exit $status
