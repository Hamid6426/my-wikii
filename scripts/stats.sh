#!/usr/bin/env bash
# Print wiki statistics: pages, lessons, lines and example files per topic, then thin pages and totals.
# Usage: ./scripts/stats.sh [path...]
#   path: a folder (wiki/, langs, langs/csharp) or a single .md page,
#         relative to the repo root or cwd, or absolute. No path means wiki/ and langs/.
#   A single file prints its own line count and thin-page verdict.
# Thin page: a README.md or lesson with fewer than THIN_MIN non-blank lines. Read-only.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
THIN_MIN=15
LESSON_RE='^[0-9][0-9]_'

# rel <abs-path>: path relative to the repo root when inside it.
rel() {
  local p="$1"
  if [[ "$p" == "$ROOT/"* ]]; then
    printf '%s' "${p#"$ROOT"/}"
  else
    printf '%s' "$p"
  fi
}

# resolve <arg>: absolute path for an argument (cwd first, then repo root), or fail.
resolve() {
  local arg="$1" p
  for p in "$arg" "$ROOT/$arg"; do
    if [[ -e "$p" ]]; then
      if [[ -d "$p" ]]; then
        (CDPATH= cd "$p" && pwd)
      else
        printf '%s/%s\n' "$(CDPATH= cd "$(dirname "$p")" && pwd)" "$(basename "$p")"
      fi
      return 0
    fi
    [[ "$arg" == /* ]] && break
  done
  return 1
}

# topic_of <rel-path>: topic key for a path relative to the repo root.
topic_of() {
  local r="$1" rest
  case "$r" in
    wiki/README.md) echo "wiki (index)" ;;
    langs/*/*)
      rest="${r#langs/}"
      echo "langs/${rest%%/*}"
      ;;
    wiki/*/*)
      rest="${r#wiki/}"
      echo "${rest%%/*}"
      ;;
    wiki/*) echo "wiki (index)" ;;
    *) echo "(outside wiki)" ;;
  esac
}

# is_lesson <rel-path>: a numbered lesson file (langs/<lang>/NN_x.md) or folder (langs/<lang>/NN_x/README.md).
is_lesson() {
  local r="$1" rest
  [[ "$r" == langs/*/* ]] || return 1
  rest="${r#langs/*/}"
  if [[ "$rest" != */* ]]; then
    [[ "$rest" =~ $LESSON_RE && "$rest" == *.md ]]
  else
    [[ "$rest" =~ $LESSON_RE && "${rest#*/}" == README.md ]]
  fi
}

# count_lines <file>: "<lines> <non-blank lines>", counting a last line without a newline.
count_lines() {
  awk '{ n++ } NF { b++ } END { print n + 0, b + 0 }' "$1"
}

# verdict <rel-path> <non-blank>: thin, ok, or n/a for pages that are not a README or lesson.
verdict() {
  local r="$1" b="$2"
  if [[ "$(basename "$r")" != README.md ]] && ! is_lesson "$r"; then
    echo "n/a (not a README or lesson)"
  elif ((b < THIN_MIN)); then
    echo "thin"
  else
    echo "ok"
  fi
}

scopes=()
if (($# == 0)); then
  scopes=("$ROOT/wiki" "$ROOT/langs")
else
  for arg in "$@"; do
    if ! p="$(resolve "$arg")"; then
      echo "Not found: $arg" >&2
      exit 1
    fi
    scopes+=("$p")
  done
fi

# Single file: its own line count and verdict.
if ((${#scopes[@]} == 1)) && [[ -f "${scopes[0]}" ]]; then
  f="${scopes[0]}"
  r="$(rel "$f")"
  read -r lines nonblank < <(count_lines "$f")
  if [[ "$f" == *.md ]]; then
    echo "$r: $lines lines, $nonblank non-blank, $(verdict "$r" "$nonblank")"
  else
    echo "$r: $lines lines, $nonblank non-blank (not a page)"
  fi
  exit 0
fi

PRUNE=(\( -name .git -o -name graft -o -name node_modules -o -name bin -o -name obj -o -name tmp \) -prune)
CODE=(\( -name '*.cs' -o -name '*.java' -o -name '*.py' -o -name '*.js' -o -name '*.mjs' -o -name '*.cjs'
  -o -name '*.ts' \))

pages_list="$(
  for s in "${scopes[@]}"; do
    if [[ -f "$s" ]]; then
      [[ "$s" == *.md ]] && printf '%s\n' "$s"
    else
      find "$s" "${PRUNE[@]}" -o -type f -name '*.md' -print
    fi
  done | sort -u
)"
examples_list="$(
  for s in "${scopes[@]}"; do
    if [[ -f "$s" ]]; then
      case "$s" in *.cs | *.java | *.py | *.js | *.mjs | *.cjs | *.ts) printf '%s\n' "$s" ;; esac
    else
      find "$s" "${PRUNE[@]}" -o -type f "${CODE[@]}" -print
    fi
  done | sort -u
)"

declare -A PAGES=() LESSONS=() LINES=() EXAMPLES=()
thin=()
total_pages=0 total_lessons=0 total_lines=0 total_examples=0

while IFS= read -r f; do
  [[ -n "$f" ]] || continue
  r="$(rel "$f")"
  t="$(topic_of "$r")"
  read -r lines nonblank < <(count_lines "$f")
  PAGES["$t"]=$((${PAGES[$t]:-0} + 1))
  LINES["$t"]=$((${LINES[$t]:-0} + lines))
  total_pages=$((total_pages + 1))
  total_lines=$((total_lines + lines))
  if is_lesson "$r"; then
    LESSONS["$t"]=$((${LESSONS[$t]:-0} + 1))
    total_lessons=$((total_lessons + 1))
  fi
  if [[ "$(verdict "$r" "$nonblank")" == thin ]]; then
    thin+=("$r: $nonblank")
  fi
done <<< "$pages_list"

while IFS= read -r f; do
  [[ -n "$f" ]] || continue
  t="$(topic_of "$(rel "$f")")"
  EXAMPLES["$t"]=$((${EXAMPLES[$t]:-0} + 1))
  PAGES["$t"]=${PAGES[$t]:-0}
  LINES["$t"]=${LINES[$t]:-0}
  total_examples=$((total_examples + 1))
done <<< "$examples_list"

# Topic order: index, linux, containers, other wiki folders, languages, then anything outside wiki.
rank() {
  case "$1" in
    "wiki (index)") echo 0 ;;
    linux) echo 1 ;;
    containers) echo 2 ;;
    langs/*) echo 4 ;;
    "(outside wiki)") echo 5 ;;
    *) echo 3 ;;
  esac
}
topics=()
if ((${#PAGES[@]} > 0)); then
  while IFS=$'\t' read -r _ t; do
    topics+=("$t")
  done < <(for t in "${!PAGES[@]}"; do printf '%s\t%s\n' "$(rank "$t")" "$t"; done | sort -t $'\t' -k1,1n -k2,2)
fi

# Lessons and examples only apply to language folders.
rows=()
rows+=("Topic"$'\t'"Pages"$'\t'"Lessons"$'\t'"Lines"$'\t'"Examples")
for t in "${topics[@]}"; do
  if [[ "$t" == langs/* ]]; then
    les="${LESSONS[$t]:-0}"
    ex="${EXAMPLES[$t]:-0}"
  else
    les="-"
    ex="-"
  fi
  rows+=("$t"$'\t'"${PAGES[$t]}"$'\t'"$les"$'\t'"${LINES[$t]}"$'\t'"$ex")
done
rows+=("Total"$'\t'"$total_pages"$'\t'"$total_lessons"$'\t'"$total_lines"$'\t'"$total_examples")

# Column widths, then an aligned Markdown table (numbers right-aligned).
w=(0 0 0 0 0)
for row in "${rows[@]}"; do
  IFS=$'\t' read -r -a cells <<< "$row"
  for i in 0 1 2 3 4; do
    if ((${#cells[i]} > w[i])); then
      w[i]=${#cells[i]}
    fi
  done
done

last=$((${#rows[@]} - 1))
for n in "${!rows[@]}"; do
  IFS=$'\t' read -r -a cells <<< "${rows[n]}"
  if ((n == last)); then
    cells[0]="**${cells[0]}**"
  fi
  printf "| %-*s | %*s | %*s | %*s | %*s |\n" \
    "$((w[0] + 4))" "${cells[0]}" "${w[1]}" "${cells[1]}" "${w[2]}" "${cells[2]}" \
    "${w[3]}" "${cells[3]}" "${w[4]}" "${cells[4]}"
  if ((n == 0)); then
    printf '| %s | %s: | %s: | %s: | %s: |\n' \
      "$(printf '%*s' "$((w[0] + 4))" '' | tr ' ' -)" \
      "$(printf '%*s' "$((w[1] - 1))" '' | tr ' ' -)" "$(printf '%*s' "$((w[2] - 1))" '' | tr ' ' -)" \
      "$(printf '%*s' "$((w[3] - 1))" '' | tr ' ' -)" "$(printf '%*s' "$((w[4] - 1))" '' | tr ' ' -)"
  fi
done

echo
echo "Thin pages (README.md or lesson under $THIN_MIN non-blank lines):"
if ((${#thin[@]} == 0)); then
  echo "  none"
else
  printf '  %s\n' "${thin[@]}"
fi

echo
echo "Totals: ${#topics[@]} topics, $total_pages pages, $total_lessons lessons, $total_lines lines," \
  "$total_examples example files, ${#thin[@]} thin pages"
