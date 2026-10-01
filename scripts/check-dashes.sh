#!/usr/bin/env bash
# Report every em dash in the repo's text files.
# Usage: ./scripts/check-dashes.sh [path...]
#   path: a file or a folder (default: the whole repo)
# Checks .md .sh .py .cs .java .js .ts .json .yml .txt files.
set -euo pipefail

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
EM_DASH=$'\xe2\x80\x94'

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

problems=0
# grep exits 1 when nothing matches, which is the clean case
while IFS= read -r hit; do
  file="${hit%%:*}"
  rest="${hit#*:}"
  lineno="${rest%%:*}"
  echo "$(show_path "$file"):$lineno: em dash"
  problems=$((problems + 1))
done < <(grep -rnHF \
  --exclude-dir=.git --exclude-dir=graft --exclude-dir=node_modules --exclude-dir=tmp \
  --include='*.md' --include='*.sh' --include='*.py' --include='*.cs' \
  --include='*.java' --include='*.js' --include='*.ts' --include='*.json' \
  --include='*.yml' --include='*.txt' \
  -- "$EM_DASH" "${targets[@]}" || true)

if ((problems > 0)); then
  echo "$problems problem(s)"
  exit 1
fi
echo "ok: no em dashes"
exit $status
