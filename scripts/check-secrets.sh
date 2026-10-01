#!/usr/bin/env bash
# Report .env files and lines that look like secrets (keys, tokens, passwords).
# Usage: ./scripts/check-secrets.sh [path...]
#   path: a file or a folder (default: the whole repo)
# Skips .git/, graft/, node_modules/ and this script.
set -euo pipefail

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SELF="$(realpath "${BASH_SOURCE[0]}")"

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

# One extended regex per secret kind, as "case|label|regex" (case: i ignores case)
PATTERNS=(
  '-|private key|-----BEGIN [A-Z ]*PRIVATE KEY'
  '-|AWS access key id|AKIA[0-9A-Z]{16}'
  '-|GitHub token|ghp_[A-Za-z0-9]{36}'
  '-|API secret key|sk-[A-Za-z0-9]{20,}'
  '-|Slack token|xox[baprs]-'
  'i|hardcoded credential|(password|passwd|secret|token|api[_-]?key)[A-Za-z0-9_]*["'\'']?[[:space:]]*[:=][[:space:]]*["'\''][^"'\'']{8,}["'\'']'
)

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

# .env files, but not .env.example
while IFS= read -r f; do
  echo "$(show_path "$f"):1: env file should not be committed"
  problems=$((problems + 1))
done < <(find "${targets[@]}" \( -name .git -o -name graft -o -name node_modules \) -prune \
  -o -type f \( -name .env -o -name '.env.*' \) ! -name .env.example -print | sort)

for entry in "${PATTERNS[@]}"; do
  case_flag="${entry%%|*}"
  entry="${entry#*|}"
  label="${entry%%|*}"
  regex="${entry#*|}"
  opts=(-rnHIE)
  [[ "$case_flag" == i ]] && opts+=(-i)
  # grep exits 1 when nothing matches, which is the clean case
  while IFS= read -r hit; do
    file="${hit%%:*}"
    rest="${hit#*:}"
    lineno="${rest%%:*}"
    [[ "$file" == "$SELF" ]] && continue
    echo "$(show_path "$file"):$lineno: possible $label"
    problems=$((problems + 1))
  done < <(grep "${opts[@]}" \
    --exclude-dir=.git --exclude-dir=graft --exclude-dir=node_modules \
    -- "$regex" "${targets[@]}" || true)
done

if ((problems > 0)); then
  echo "$problems problem(s)"
  exit 1
fi
echo "ok: no secrets found"
exit $status
