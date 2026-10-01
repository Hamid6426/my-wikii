#!/usr/bin/env bash
# Insert a new numbered lesson in a language folder and renumber the lessons after it.
# Usage: ./scripts/insert-lesson.sh <lang> <NN> "<Title>"
#   lang:  html-and-css javascript typescript csharp java python
#          (also accepted as langs/<lang> or wiki/langs/<lang>)
#   NN:    the position to insert at, two digits, 01 to <highest lesson + 1>
#   Title: the lesson title, used for the H1, the file name and the README row
# Creates wiki/langs/<lang>/NN_topic_name.md with just the H1, shifts every lesson from
# NN upward by one, and rewrites the H1s, links, example file names and README table.
# Backs up the folder first (see backup-lang.sh). Run it again to insert a second lesson.
# WIKI_ROOT overrides the wiki folder (default: <repo>/wiki).
set -euo pipefail
export LC_ALL=C
shopt -s nullglob

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WIKI="${WIKI_ROOT:-$ROOT/wiki}"
README_NAME="README.md"
LANGS=(html-and-css javascript typescript csharp java python)

die() {
  echo "insert-lesson: $*" >&2
  exit 1
}

if (($# != 3)); then
  echo "Usage: $0 <lang> <NN> \"<Title>\"" >&2
  echo "  lang: ${LANGS[*]}" >&2
  echo "  NN:   two digits, 01 to <highest lesson + 1>" >&2
  exit 1
fi

lang="$1"
nn="$2"
title="$3"

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

[[ "$nn" =~ ^[0-9]{2}$ ]] || die "position must be two digits, such as 18"
pos=$((10#$nn))

title="${title#"${title%%[![:space:]]*}"}"
title="${title%"${title##*[![:space:]]}"}"
[[ -n "$title" ]] || die "title is empty"
[[ "$title" != *$'\n'* ]] || die "title must be one line"
[[ "$title" != *"|"* ]] || die "title must not contain '|' (it breaks the Lessons table)"

slug="$(tr '[:upper:]' '[:lower:]' <<<"$title" | sed -E 's/[^a-z0-9]+/_/g; s/^_+//; s/_+$//')"
[[ -n "$slug" ]] || die "title has no letters or digits to build a file name from"

dir="$WIKI/langs/$lang"
[[ -d "$dir" ]] || die "language folder not found: wiki/langs/$lang"
[[ -f "$dir/$README_NAME" ]] || die "language index not found: wiki/langs/$lang/$README_NAME"

# Collect the existing lessons: highest number, and the slug of each number.
declare -A slug_of=()
max=0
for f in "$dir"/[0-9][0-9]_*.md; do
  b="${f##*/}"
  n=$((10#${b:0:2}))
  slug_of["$n"]="${b:3}"
  slug_of["$n"]="${slug_of["$n"]%.md}"
  ((n > max)) && max=$n
done
((max > 0)) || die "no numbered lessons found in wiki/langs/$lang"

((max < 99)) || die "lesson numbers are two digits and 99 is taken"
((pos >= 1 && pos <= max + 1)) || die "position $nn is out of range: use 01 to $(printf '%02d' $((max + 1)))"

file="${nn}_${slug}.md"
[[ ! -e "$dir/$file" ]] || die "already exists: wiki/langs/$lang/$file"
for f in "$dir"/[0-9][0-9]_"$slug".md; do
  die "a lesson with this name exists at another number: ${f##*/}"
done

shifts=$((max - pos + 1))

# 1. Back up the folder so a mistake can be undone.
"$ROOT/scripts/backup-lang.sh" "$lang"

# 2. Build the literal replacement map, highest lesson first so no replacement cascades.
mapf="$(mktemp)"
trap 'rm -f "$mapf"' EXIT
for ((n = max; n >= pos; n--)); do
  o="$(printf '%02d' "$n")"
  w="$(printf '%02d' $((n + 1)))"
  s="${slug_of["$n"]}"
  printf '%s\t%s\n' "${o}_${s}.md" "${w}_${s}.md" >>"$mapf"
  printf '%s\t%s\n' "# ${o} - " "# ${w} - " >>"$mapf"
  printf '%s\t%s\n' "lesson ${o}" "lesson ${w}" >>"$mapf"
  printf '%s\t%s\n' "Lesson ${o}" "Lesson ${w}" >>"$mapf"
  for ex in "$dir/examples/${o}-"*.java "$dir/examples/${o}-"*.txt; do
    eb="${ex##*/}"
    printf '%s\t%s\n' "$eb" "${w}-${eb:3}" >>"$mapf"
  done
done

# 3. Rename the example files, then the lesson files, from the highest number down.
for ((n = max; n >= pos; n--)); do
  o="$(printf '%02d' "$n")"
  w="$(printf '%02d' $((n + 1)))"
  for ex in "$dir/examples/${o}-"*.java "$dir/examples/${o}-"*.txt; do
    eb="${ex##*/}"
    mv -- "$ex" "$dir/examples/${w}-${eb:3}"
  done
done
for ((n = max; n >= pos; n--)); do
  o="$(printf '%02d' "$n")"
  w="$(printf '%02d' $((n + 1)))"
  mv -- "$dir/${o}_${slug_of["$n"]}.md" "$dir/${w}_${slug_of["$n"]}.md"
done

# 4. Rewrite every reference (links, H1s, example names, "lesson NN" text), tmp excluded.
find "$dir" -name tmp -prune -o -type f -print0 |
  while IFS= read -r -d '' f; do
    case "$f" in
      *.md | *.java | *.txt) ;;
      *) continue ;;
    esac
    awk -v mapfile="$mapf" '
      BEGIN {
        n = 0
        while ((getline line < mapfile) > 0) {
          if (line == "") continue
          i = index(line, "\t")
          if (i == 0) continue
          old[n] = substr(line, 1, i - 1)
          new[n] = substr(line, i + 1)
          n++
        }
        close(mapfile)
      }
      {
        s = $0
        for (i = 0; i < n; i++) {
          o = old[i]
          if (o == "" || index(s, o) == 0) continue
          res = ""
          rest = s
          while ((p = index(rest, o)) > 0) {
            res = res substr(rest, 1, p - 1) new[i]
            rest = substr(rest, p + length(o))
          }
          s = res rest
        }
        print s
      }
    ' "$f" >"$f.mapit"
    cat "$f.mapit" >"$f"
    rm -f "$f.mapit"
  done

# 5. Create the new lesson with just the H1, ready to fill in.
printf '# %s - %s\n' "$nn" "$title" >"$dir/$file"

# 6. Rebuild the Lessons table in the language README from the lesson files.
table="$(mktemp)"
out="$(mktemp)"
trap 'rm -f "$mapf" "$table" "$out"' EXIT
{
  printf '| No. | Lesson |\n'
  printf '| --- | --- |\n'
  for f in "$dir"/[0-9][0-9]_*.md; do
    b="${f##*/}"
    num="${b:0:2}"
    h1="$(awk '
      /^[ \t]*(```|~~~)/ { fence = !fence; next }
      fence { next }
      /^# / { sub(/^# /, ""); print; exit }
    ' "$f")"
    printf '| %s | [%s](%s) |\n' "$num" "${h1#"$num" - }" "$b"
  done
} >"$table"
awk -v tf="$table" '
  BEGIN { while ((getline l < tf) > 0) tbl = tbl l "\n" }
  { lines[++n] = $0 }
  END {
    state = 0
    for (i = 1; i <= n; i++) {
      if (state == 0) {
        print lines[i]
        if (lines[i] ~ /^## Lessons[ \t]*$/) state = 1
        continue
      }
      if (state == 1) {
        if (lines[i] ~ /^[ \t]*$/ || lines[i] ~ /^\|/) continue
        printf "%s", tbl
        print ""
        state = 2
      }
      print lines[i]
    }
    if (state == 1) printf "%s", tbl
  }
' "$dir/$README_NAME" >"$out"
cat "$out" >"$dir/$README_NAME"

# 7. Keep the lesson count in the README prose in step.
total=$((max + 1))
if grep -qE 'in [0-9]+ short lessons' "$dir/$README_NAME"; then
  sed -i -E "s/in [0-9]+ short lessons/in ${total} short lessons/" "$dir/$README_NAME"
fi

# 8. Align the tables.
"$ROOT/scripts/format.sh" "$dir" >/dev/null

echo "Inserted:   wiki/langs/$lang/$file"
echo "Renumbered: $shifts lesson(s) from $(printf '%02d' "$pos") upward"
echo "Backup:     wiki/langs/$lang/tmp/"
echo "Next:       write the lesson body, then add examples/${nn}-01_*.java or examples/${nn}-00_no_examples.txt"

