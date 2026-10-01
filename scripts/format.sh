#!/usr/bin/env bash
# Format Markdown files.
# Also aligns tables: every cell is padded to the widest cell in its column
# and the --- row is resized to match. Tables inside code fences are left alone.
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

# Align Markdown tables: pad every cell so the pipes line up. Code fences are skipped.
align_tables() {
  LC_ALL=C.UTF-8 awk '
    function trim(x) { gsub(/^[ \t]+|[ \t]+$/, "", x); return x }
    function flush(   r, c, i, line, cell, pad, a, dash) {
      for (r = 1; r <= n; r++) for (c = 1; c <= nc[r]; c++) if (!(isdel[r])) if (length(cells[r, c]) > width[c]) width[c] = length(cells[r, c])
      for (c = 1; c <= maxc; c++) if (width[c] < 3) width[c] = 3
      for (r = 1; r <= n; r++) {
        line = "|"
        for (c = 1; c <= maxc; c++) {
          cell = (c <= nc[r]) ? cells[r, c] : ""
          if (isdel[r]) {
            a = cell
            dash = ""
            for (i = 0; i < width[c]; i++) dash = dash "-"
            if (a ~ /^:/) dash = ":" substr(dash, 2)
            if (a ~ /:$/) dash = substr(dash, 1, length(dash) - 1) ":"
            line = line " " dash " |"
          } else {
            pad = width[c] - length(cell)
            line = line " " cell sprintf("%*s", pad, "") " |"
          }
        }
        print line
      }
      n = 0; maxc = 0; delete width; delete cells; delete nc; delete isdel
    }
    /^[ \t]*(```|~~~)/ { if (n) flush(); fence = !fence; print; next }
    fence { print; next }
    /^\|.*\|[ \t]*$/ {
      line = trim($0); gsub(/\\\|/, "\001", line)
      line = substr(line, 2, length(line) - 2)
      k = split(line, parts, "|")
      n++; nc[n] = k; if (k > maxc) maxc = k
      isdel[n] = 1
      for (c = 1; c <= k; c++) {
        t = trim(parts[c]); gsub(/\001/, "\\|", t); cells[n, c] = t
        if (t !~ /^:?-+:?$/) isdel[n] = 0
      }
      next
    }
    { if (n) flush(); print }
    END { if (n) flush() }
  '
}

format_file() {
  local file="$1" tmp
  tmp=$(mktemp)
  format_markdown < "$file" | align_tables > "$tmp"
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
    done < <(find "$target" \( -name .git -o -name graft -o -name node_modules -o -name tmp \) -prune -o -type f -name '*.md' -print0 | sort -z)
  elif [[ -f "$target" ]]; then
    format_file "$target"
  else
    echo "Not found: $target" >&2
    status=1
  fi
done
exit $status
