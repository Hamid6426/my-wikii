#!/usr/bin/env bash
# Syntax or compile check the example code in the wiki. Build output goes to a temp dir, never the repo.
# Usage: ./scripts/check-examples.sh [path...]
#   path: a folder (wiki/, langs, langs/csharp/examples) or a single example file
#         (.cs, .java, .py, .js, .ts), relative to the repo root or cwd, or absolute.
#         No path means wiki/ and langs/.
# Tools: dotnet (.cs), javac (.java), python3 (.py), node (.js), tsc (.ts).
#   A missing tool skips that language and is not a failure.
# Exit: 0 when everything checked passes, 1 on any failure or a missing path.
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

TMP="$(mktemp -d)"
trap 'rm -rf "$TMP"' EXIT

export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export PYTHONDONTWRITEBYTECODE=1
export PYTHONPYCACHEPREFIX="$TMP/pycache"

JOBS="$(nproc 2>/dev/null || echo 4)"

checked=0
failed=0
skipped=0

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

# lang_of <file>: language name for a code file, empty if not an example file.
lang_of() {
  case "$1" in
    *.cs) echo csharp ;;
    *.java) echo java ;;
    *.py) echo python ;;
    *.js | *.mjs | *.cjs) echo javascript ;;
    *.ts) echo typescript ;;
    *) echo "" ;;
  esac
}

# Collect example files in scope.
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

declare -A FILES=()
for s in "${scopes[@]}"; do
  if [[ -f "$s" ]]; then
    lang="$(lang_of "$s")"
    if [[ -z "$lang" ]]; then
      echo "skip $(rel "$s"): not an example file"
      continue
    fi
    FILES["$lang"]+="$s"$'\n'
    continue
  fi
  while IFS= read -r -d '' f; do
    lang="$(lang_of "$f")"
    FILES["$lang"]+="$f"$'\n'
  done < <(find "$s" \( -name .git -o -name graft -o -name node_modules -o -name bin -o -name obj -o -name tmp \) -prune \
    -o -type f \( -name '*.cs' -o -name '*.java' -o -name '*.py' -o -name '*.js' -o -name '*.mjs' \
    -o -name '*.cjs' -o -name '*.ts' \) ! -name '*.d.ts' -print0)
done

# files_for <lang>: sorted, de-duplicated file list for a language, one per line.
files_for() {
  printf '%s' "${FILES[$1]:-}" | sed '/^$/d' | sort -u
}

# report <errors>: count one failed file and print its error lines.
report() {
  local errors="$1"
  failed=$((failed + 1))
  printf '%s\n' "$errors"
}

check_csharp() {
  local list=("$@") i=0 f name dir
  if ! command -v dotnet > /dev/null; then
    echo "skip csharp: dotnet not installed"
    skipped=$((skipped + ${#list[@]}))
    return
  fi
  echo "csharp: checking ${#list[@]} files with dotnet build"
  # Each sample has its own top-level statements, so each one gets its own throwaway project.
  # 'dotnet project convert' turns '#:package' and '#:property' directives into a real .csproj.
  # The original file then replaces the converted copy (which has the directives stripped), so
  # error line numbers match the repo file. Features=FileBasedProgram lets the compiler accept '#:'.
  for f in "${list[@]}"; do
    i=$((i + 1))
    dir="$TMP/cs/$i"
    name="$(basename "$f")"
    mkdir -p "$dir/src"
    cp "$f" "$dir/src/$name"
    (
      if ! dotnet project convert "$dir/src/$name" -o "$dir/proj" > "$dir/log" 2>&1; then
        echo 1 > "$dir/status"
        exit 0
      fi
      cp "$f" "$dir/proj/$name"
      if dotnet build "$dir/proj" -nologo -v q -clp:NoSummary \
        -p:Features=FileBasedProgram > "$dir/log" 2>&1; then
        echo 0 > "$dir/status"
      else
        echo 1 > "$dir/status"
      fi
    ) &
    while (($(jobs -rp | wc -l) >= JOBS)); do
      wait -n || true
    done
  done
  wait

  i=0
  local errors
  for f in "${list[@]}"; do
    i=$((i + 1))
    dir="$TMP/cs/$i"
    name="$(basename "$f")"
    checked=$((checked + 1))
    if [[ "$(cat "$dir/status" 2> /dev/null || echo 1)" != 0 ]]; then
      errors="$(grep -E '(^|: )error ' "$dir/log" \
        | sed -e "s#$dir/\(src\|proj\)/$name#$(rel "$f")#g" -e 's# \[[^]]*\.csproj\]$##' \
        | sort -u || true)"
      [[ -n "$errors" ]] || errors="$(rel "$f"): error: $(tail -n 1 "$dir/log")"
      report "$errors"
    fi
  done
}

check_java() {
  local list=("$@") f d i=0 out errors
  if ! command -v javac > /dev/null; then
    echo "skip java: javac not installed"
    skipped=$((skipped + ${#list[@]}))
    return
  fi
  echo "java: checking ${#list[@]} files with javac"
  # Compile per source folder: several folders each have their own Main class.
  # -sourcepath lets a single file find its sibling classes.
  local dirs
  dirs="$(printf '%s\n' "${list[@]}" | xargs -d '\n' -n 1 dirname | sort -u)"
  while IFS= read -r d; do
    i=$((i + 1))
    out="$TMP/java/$i"
    mkdir -p "$out"
    local group=()
    for f in "${list[@]}"; do
      [[ "$(dirname "$f")" == "$d" ]] && group+=("$f")
    done
    checked=$((checked + ${#group[@]}))
    if ! javac -d "$out" -sourcepath "$d" -nowarn "${group[@]}" > "$out.log" 2>&1; then
      errors="$(grep -E ': error: ' "$out.log" | sed "s#$ROOT/##" || true)"
      [[ -n "$errors" ]] || errors="$(rel "$d"): error: $(tail -n 1 "$out.log")"
      # Count one failure per file that has an error.
      local bad
      bad="$(printf '%s\n' "$errors" | cut -d: -f1 | sort -u | wc -l)"
      printf '%s\n' "$errors"
      failed=$((failed + bad))
    fi
  done <<< "$dirs"
}

check_python() {
  local list=("$@")
  if ! command -v python3 > /dev/null; then
    echo "skip python: python3 not installed"
    skipped=$((skipped + ${#list[@]}))
    return
  fi
  echo "python: checking ${#list[@]} files with py_compile"
  checked=$((checked + ${#list[@]}))
  local errors bad
  # One interpreter for all files; bytecode goes to the temp dir only.
  errors="$(python3 - "$ROOT" "$TMP/pyc" "${list[@]}" << 'PY'
import os, py_compile, sys
root, out, files = sys.argv[1], sys.argv[2], sys.argv[3:]
os.makedirs(out, exist_ok=True)
for i, f in enumerate(files):
    rel = os.path.relpath(f, root) if f.startswith(root + os.sep) else f
    try:
        py_compile.compile(f, cfile=os.path.join(out, f"{i}.pyc"), doraise=True)
    except py_compile.PyCompileError as e:
        line = getattr(e.exc_value, "lineno", None)
        msg = getattr(e.exc_value, "msg", str(e.exc_value))
        where = f"{rel}:{line}" if line else rel
        print(f"{where}: error: {e.exc_type_name}: {msg}")
PY
)"
  if [[ -n "$errors" ]]; then
    printf '%s\n' "$errors"
    bad="$(printf '%s\n' "$errors" | cut -d: -f1 | sort -u | wc -l)"
    failed=$((failed + bad))
  fi
}

check_javascript() {
  local list=("$@") f log msg
  if ! command -v node > /dev/null; then
    echo "skip javascript: node not installed"
    skipped=$((skipped + ${#list[@]}))
    return
  fi
  echo "javascript: checking ${#list[@]} files with node --check"
  log="$TMP/node.log"
  for f in "${list[@]}"; do
    checked=$((checked + 1))
    if ! node --check "$f" > "$log" 2>&1; then
      # node prints 'path:line', the source line, a caret, then 'SyntaxError: ...'.
      msg="$(grep -m 1 -E '^[A-Za-z]*Error' "$log" || tail -n 1 "$log")"
      report "$(rel "$(head -n 1 "$log")"): error: $msg"
    fi
  done
}

check_typescript() {
  local list=("$@") f log errors
  if ! command -v tsc > /dev/null; then
    echo "skip typescript: tsc not installed"
    skipped=$((skipped + ${#list[@]}))
    return
  fi
  echo "typescript: checking ${#list[@]} files with tsc --noEmit"
  log="$TMP/tsc.log"
  # One file at a time: script files without imports share a global scope and would clash.
  for f in "${list[@]}"; do
    checked=$((checked + 1))
    if ! tsc --noEmit --pretty false --skipLibCheck --target es2022 "$f" > "$log" 2>&1; then
      errors="$(grep -E 'error TS' "$log" | sed "s#$ROOT/##" || true)"
      [[ -n "$errors" ]] || errors="$(rel "$f"): error: $(tail -n 1 "$log")"
      report "$errors"
    fi
  done
}

for lang in csharp java python javascript typescript; do
  list=()
  while IFS= read -r f; do
    [[ -n "$f" ]] && list+=("$f")
  done < <(files_for "$lang")
  ((${#list[@]} > 0)) || continue
  "check_$lang" "${list[@]}"
done

echo "checked $checked files: $((checked - failed)) passed, $failed failed, $skipped skipped"
((failed == 0))
