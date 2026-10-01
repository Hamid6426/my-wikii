#!/usr/bin/env bash
# Format Markdown files.
# Usage: ./format.sh <path>...
#   path: a .md file, or a folder (all .md files under it, recursively)
# Examples: ./format.sh wiki/   ./format.sh wiki/linux   ./format.sh wiki/linux/README.md

format_markdown() {
  local fence=""
  local consecutive_blanks=0
  local line stripped indent hashes title

  while IFS= read -r line || [[ -n "$line" ]]; do
    stripped="${line#"${line%%[![:space:]]*}"}"
    stripped="${stripped%"${stripped##*[![:space:]]}"}"

    # Fenced code blocks: copy verbatim, close only on the same marker
    if [[ "$stripped" =~ ^(\`\`\`|~~~) ]]; then
      local marker="${BASH_REMATCH[1]}"
      if [[ -z "$fence" ]]; then
        fence="$marker"
      elif [[ "$fence" == "$marker" ]]; then
        fence=""
      fi
      echo "${line%"${line##*[![:space:]]}"}"
      consecutive_blanks=0
      continue
    fi

    if [[ -n "$fence" ]]; then
      echo "$line"
      continue
    fi

    # Blank lines: keep at most one
    if [[ -z "$stripped" ]]; then
      consecutive_blanks=$((consecutive_blanks + 1))
      if ((consecutive_blanks == 1)); then
        echo ""
      fi
      continue
    fi
    consecutive_blanks=0

    # Keep leading indentation for nested blocks
    indent=""
    if [[ "$line" =~ ^([[:space:]]+) ]]; then
      indent="${BASH_REMATCH[1]}"
    fi

    if [[ "$stripped" =~ ^(#{1,6})[[:space:]]+(.*)$ ]]; then
      # Headings: exactly one space after the hashes
      hashes="${BASH_REMATCH[1]}"
      title="${BASH_REMATCH[2]}"
      stripped="${hashes} ${title}"
    elif [[ "$stripped" =~ ^[\*\+][[:space:]]+(.*)$ ]]; then
      # Bullets: '*' and '+' become '-'
      stripped="- ${BASH_REMATCH[1]}"
    elif [[ "$stripped" =~ ^([0-9]+\.)[[:space:]]+(.*)$ ]]; then
      # Numbered lists: exactly one space after the dot
      stripped="${BASH_REMATCH[1]} ${BASH_REMATCH[2]}"
    elif [[ "$stripped" =~ ^\>[[:space:]]*(.+)$ ]]; then
      # Blockquotes: one space after '>'
      stripped="> ${BASH_REMATCH[1]}"
    fi

    echo "${indent}${stripped}"
  done
}

format_file() {
  local file="$1" tmp
  tmp=$(mktemp)
  format_markdown < "$file" > "$tmp"
  # Overwrite in place so file permissions are kept
  cat "$tmp" > "$file"
  rm -f "$tmp"
  echo "Formatted: $file"
}

if [[ $# -eq 0 ]]; then
  echo "Usage: $0 <file.md | folder>..."
  exit 1
fi

status=0
for target in "$@"; do
  if [[ -d "$target" ]]; then
    while IFS= read -r -d '' f; do
      format_file "$f"
    done < <(find "$target" -type f -name '*.md' -print0 | sort -z)
  elif [[ -f "$target" ]]; then
    format_file "$target"
  else
    echo "Not found: $target" >&2
    status=1
  fi
done
exit $status
