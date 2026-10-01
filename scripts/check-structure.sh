#!/usr/bin/env bash
# Check that the wiki follows the layout rules in AGENTS.md and CONTRIBUTING.md.
# Usage: ./scripts/check-structure.sh [path...]
#   path: a folder (wiki/, wiki/linux, wiki/langs/csharp) or a single file (wiki/linux/README.md),
#         relative to the repo root or the current folder, or absolute. No path means all of wiki/.
#   Only problems in files inside the given paths are printed. Parent and language README files
#   are still read to decide whether a page or lesson is linked.
#   WIKI_ROOT=<dir> overrides the wiki folder (default: <repo>/wiki), for testing on a copy.
# Examples: ./scripts/check-structure.sh   ./scripts/check-structure.sh wiki/langs/csharp
# Checks:
#   1. Outside language folders, every .md file is a README.md (no stray topic files).
#   2. Every README.md has an H1 and a Tags: line right under it (one blank line allowed).
#   3. Every README.md except the wiki root is linked from the nearest parent README.md.
#   4. Language folders hold flat NN_topic_name.md lessons with a matching "# NN - Title" H1,
#      each linked from the language README.md, numbered with no gaps or duplicates.
# Prints "path:line: message" per problem and exits 1 if there are any, else exits 0.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
README_NAME="README.md"
LANGS_DIR="langs"
LANGUAGES=(html css javascript typescript csharp java python)
# Folders allowed inside a language folder that are not lessons.
NON_LESSON_DIRS=(examples algorithms 888_algorithms docs src)
LESSON_RE='^([0-9]{2})_[a-z0-9]+(_[a-z0-9]+)*\.md$'

WIKI="${WIKI_ROOT:-$ROOT/wiki}"
if [[ ! -d "$WIKI" ]]; then
  echo "Not found: $WIKI" >&2
  exit 1
fi
WIKI="$(CDPATH= cd "$WIKI" && pwd)"

# display <abs>: path relative to the repo root when inside it, else absolute.
display() {
  case "$1" in
    "$ROOT"/*) printf '%s' "${1#"$ROOT"/}" ;;
    *) printf '%s' "$1" ;;
  esac
}

# Scope: absolute, normalised paths to report on.
SCOPES=()
if (($# == 0)); then
  SCOPES=("$WIKI")
else
  status=0
  for arg in "$@"; do
    found=""
    if [[ "$arg" == /* ]]; then
      [[ -e "$arg" ]] && found="$arg"
    else
      # The wiki's parent comes before the repo root so WIKI_ROOT copies resolve "wiki/..." to themselves.
      for base in "$PWD" "$(dirname "$WIKI")" "$ROOT"; do
        if [[ -e "$base/$arg" ]]; then
          found="$base/$arg"
          break
        fi
      done
    fi
    if [[ -z "$found" ]]; then
      echo "Not found: $arg" >&2
      status=1
      continue
    fi
    found="$(realpath -ms "$found")"
    if [[ "$found" != "$WIKI" && "$found" != "$WIKI"/* ]]; then
      echo "Not inside $(display "$WIKI"): $arg" >&2
      status=1
      continue
    fi
    SCOPES+=("$found")
  done
  if ((status != 0)); then
    exit "$status"
  fi
fi

# in_scope <abs>: true when the path is a scope file or sits under a scope folder.
in_scope() {
  local s
  for s in "${SCOPES[@]}"; do
    if [[ "$1" == "$s" || "$1" == "$s"/* ]]; then
      return 0
    fi
  done
  return 1
}

PROBLEMS=()
# problem <rel-to-wiki> <line> <message>
problem() {
  local abs="$WIKI${1:+/$1}"
  in_scope "$abs" || return 0
  PROBLEMS+=("$(display "$abs"):$2: $3")
}

# language_of <rel>: the language folder holding a path, or nothing.
language_of() {
  local name
  for name in "${LANGUAGES[@]}"; do
    case "$1" in
      "$LANGS_DIR/$name" | "$LANGS_DIR/$name"/*)
        printf '%s' "$LANGS_DIR/$name"
        return
        ;;
    esac
  done
}

is_non_lesson_dir() {
  local name
  for name in "${NON_LESSON_DIRS[@]}"; do
    [[ "$1" == "$name" ]] && return 0
  done
  return 1
}

# dir_of <rel>: folder of a path under the wiki, "" for the wiki root.
dir_of() {
  case "$1" in
    */*) printf '%s' "${1%/*}" ;;
    *) printf '' ;;
  esac
}

# link_targets <dir-rel>: every relative link in <dir>/README.md, resolved to a path
# under the wiki. Links to a README.md come out as its folder ("" for the wiki root).
# Fenced code blocks, inline code and external URLs are ignored.
link_targets() {
  awk -v dir="$1" '
    function emit(t,    n, parts, stack, depth, i, out) {
      gsub(/^[ \t]+|[ \t]+$/, "", t)
      if (t ~ /^</) { sub(/^</, "", t); sub(/>.*$/, "", t) }
      sub(/[ \t].*$/, "", t)
      sub(/[#?].*$/, "", t)
      if (t == "" || t ~ /^\// || t ~ /^[A-Za-z][A-Za-z0-9+.-]*:/) return
      gsub(/%20/, " ", t)
      if (dir != "") t = dir "/" t
      n = split(t, parts, "/")
      depth = 0
      for (i = 1; i <= n; i++) {
        if (parts[i] == "" || parts[i] == ".") continue
        if (parts[i] == ".." && depth > 0 && stack[depth] != "..") { depth--; continue }
        stack[++depth] = parts[i]
      }
      out = ""
      for (i = 1; i <= depth; i++) out = out (i > 1 ? "/" : "") stack[i]
      if (out == "README.md") out = ""
      else sub(/\/README\.md$/, "", out)
      print out
    }
    /^[ \t]*(```|~~~)/ { fence = !fence; next }
    fence { next }
    {
      line = $0
      gsub(/`[^`]*`/, "", line)
      if (line ~ /^ {0,3}\[[^]]+\]:[ \t]+[^ \t]/) {
        t = line
        sub(/^ {0,3}\[[^]]+\]:[ \t]+/, "", t)
        emit(t)
        next
      }
      while (match(line, /\]\([^)]*\)/)) {
        emit(substr(line, RSTART + 2, RLENGTH - 3))
        line = substr(line, RSTART + RLENGTH)
      }
    }
  ' "$WIKI/${1:+$1/}$README_NAME"
}

# Links found per README folder: LINKS["<readme-dir>"TAB"<target>"]=1
declare -A LINKS
declare -A LINKS_READ
load_links() {
  local dir="$1" target
  # Prefixed key: bash rejects an empty subscript, and the wiki root folder is "".
  [[ -n "${LINKS_READ["@$dir"]+x}" ]] && return 0
  LINKS_READ["@$dir"]=1
  while IFS= read -r target; do
    LINKS["$dir"$'\t'"$target"]=1
  done < <(link_targets "$dir")
}

# check_header <rel>: rule 2, an H1 followed by a Tags: line.
check_header() {
  local rel="$1" result
  result="$(awk '
    /^[ \t]*(```|~~~)/ { fence = !fence; next }
    fence { next }
    !h && /^# [^ \t]/ { h = NR; next }
    h && NR == h + 1 { if ($0 ~ /^Tags:/) { ok = 1; exit } if ($0 != "") exit; next }
    h && NR == h + 2 { if ($0 ~ /^Tags:/) ok = 1; exit }
    END {
      if (!h) print "1\tmissing H1 (# Title)"
      else if (!ok) print (h + 1) "\tmissing Tags: line under the H1"
    }
  ' "$WIKI/$rel")"
  if [[ -n "$result" ]]; then
    problem "$rel" "${result%%$'\t'*}" "${result#*$'\t'}"
  fi
}

# check_parent_link <rel>: rule 3, linked from the nearest parent README.md.
check_parent_link() {
  local rel="$1" folder parent
  folder="$(dir_of "$rel")"
  parent="$folder"
  while [[ -n "$parent" ]]; do
    parent="$(dir_of "$parent")"
    [[ -f "$WIKI/${parent:+$parent/}$README_NAME" ]] && break
  done
  if [[ ! -f "$WIKI/${parent:+$parent/}$README_NAME" ]]; then
    problem "$rel" 1 "no parent $README_NAME to link this page from"
    return
  fi
  load_links "$parent"
  if [[ -z "${LINKS["$parent"$'\t'"$folder"]+x}" ]]; then
    problem "$rel" 1 "not linked from $(display "$WIKI/${parent:+$parent/}$README_NAME")"
  fi
}

# check_language <lang-rel>: rule 4 for one language folder.
check_language() {
  local lang="$1" entry name rel nn h1 i max=0
  local -A seen=()
  local has_readme=0
  [[ -f "$WIKI/$lang/$README_NAME" ]] && has_readme=1
  ((has_readme)) && load_links "$lang"

  while IFS= read -r -d '' entry; do
    name="${entry##*/}"
    rel="$lang/$name"
    if [[ -d "$entry" ]]; then
      [[ "$name" == .* ]] && continue
      if [[ "$name" =~ ^[0-9] ]] && ! is_non_lesson_dir "$name"; then
        problem "$rel" 1 "lesson folder, expected flat NN_topic.md"
      fi
      continue
    fi
    [[ "$name" == *.md && "$name" != "$README_NAME" ]] || continue
    if [[ ! "$name" =~ $LESSON_RE ]]; then
      problem "$rel" 1 "lesson file name must be NN_topic_name.md (two digits, lowercase, underscores)"
      continue
    fi
    nn="${BASH_REMATCH[1]}"
    if [[ -n "${seen[$nn]+x}" ]]; then
      problem "$rel" 1 "duplicate lesson number $nn (also ${seen[$nn]})"
    else
      seen["$nn"]="$name"
    fi
    ((10#$nn > max)) && max=$((10#$nn))

    h1="$(awk '
      /^[ \t]*(```|~~~)/ { fence = !fence; next }
      fence { next }
      /^# / { print NR "\t" $0; exit }
    ' "$entry")"
    if [[ -z "$h1" ]]; then
      problem "$rel" 1 "missing H1, expected \"# $nn - Title\""
    elif [[ ! "${h1#*$'\t'}" =~ ^#\ $nn\ -\ [^[:space:]] ]]; then
      problem "$rel" "${h1%%$'\t'*}" "H1 must be \"# $nn - Title\""
    fi

    if ((has_readme == 0)); then
      problem "$rel" 1 "no $(display "$WIKI/$lang/$README_NAME") to link this lesson from"
    elif [[ -z "${LINKS["$lang"$'\t'"$rel"]+x}" ]]; then
      problem "$rel" 1 "not linked from $(display "$WIKI/$lang/$README_NAME")"
    fi
  done < <(find "$WIKI/$lang" -mindepth 1 -maxdepth 1 -print0 | sort -z)

  for ((i = 1; i <= max; i++)); do
    nn="$(printf '%02d' "$i")"
    if [[ -z "${seen[$nn]+x}" ]]; then
      problem "$lang/$README_NAME" 1 "lesson numbering has a gap: no lesson $nn"
    fi
  done
}

while IFS= read -r -d '' path; do
  rel="${path#"$WIKI"/}"
  name="${rel##*/}"
  if [[ "$name" != "$README_NAME" ]]; then
    if [[ -z "$(language_of "$rel")" ]]; then
      problem "$rel" 1 "stray topic file, pages must be folder/$README_NAME"
    fi
    continue
  fi
  check_header "$rel"
  [[ "$rel" == "$README_NAME" ]] || check_parent_link "$rel"
done < <(find "$WIKI" \( -name .git -o -name graft \) -prune -o -type f -name '*.md' -print0 | sort -z)

for name in "${LANGUAGES[@]}"; do
  if [[ -d "$WIKI/$LANGS_DIR/$name" ]]; then
    check_language "$LANGS_DIR/$name"
  fi
done

if ((${#PROBLEMS[@]} == 0)); then
  echo "ok: no structure problems"
  exit 0
fi
printf '%s\n' "${PROBLEMS[@]}" | sort -t: -k1,1 -k2,2n
echo "${#PROBLEMS[@]} problem(s)"
exit 1
