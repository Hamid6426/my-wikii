#!/usr/bin/env bash
# Add Tags: lines to wiki pages and regenerate the table of contents inside wiki/README.md.
# Usage: ./scripts/sync-tags.sh [path...]
#   path: a README.md file or a folder. Only pages inside the scope get their Tags: line
#   rewritten; the table of contents in wiki/README.md always covers every page.
# Examples: ./scripts/sync-tags.sh   ./scripts/sync-tags.sh wiki/linux   ./scripts/sync-tags.sh wiki/linux/README.md
set -euo pipefail
export LC_ALL=C

ROOT="$(CDPATH= cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)/wiki"
README_NAME="README.md"
README_SUFFIX="/$README_NAME"
TOC_START="<!-- toc:start -->"
TOC_END="<!-- toc:end -->"

LINUX_DIR="linux/"
CONTAINERS_DIR="containers/"

ACCOUNTS_DIR="${LINUX_DIR}accounts/"
DESKTOP_ENVIRONMENTS_DIR="${LINUX_DIR}desktop-environments/"
DISPLAY_MANAGERS_DIR="${LINUX_DIR}display-managers/"
DISPLAY_SERVERS_DIR="${LINUX_DIR}display-servers/"
DISTROS_DIR="${LINUX_DIR}distros/"
LINUX_KERNEL_DIR="${LINUX_DIR}linux-kernel/"
NETWORKING_DIR="${LINUX_DIR}networking/"
PACKAGE_MANAGERS_DIR="${LINUX_DIR}package-managers/"
RESOURCES_DIR="${LINUX_DIR}resources/"
SHELLS_AND_TERMINALS_DIR="${LINUX_DIR}shells-and-terminals/"
SYSTEMD_DIR="${LINUX_DIR}systemd/"
WINDOW_MANAGERS_DIR="${LINUX_DIR}window-managers/"

DOCKER_DIR="${CONTAINERS_DIR}docker/"
PODMAN_DIR="${CONTAINERS_DIR}podman/"
RUNTIMES_DIR="${CONTAINERS_DIR}runtimes/"
IMAGE_BUILDERS_DIR="${CONTAINERS_DIR}image-builders/"
DESKTOP_INTERFACES_DIR="${CONTAINERS_DIR}desktop-interfaces/"

WIKI_README="$README_NAME"
LINUX_README="${LINUX_DIR}${README_NAME}"
CONTAINERS_README="${CONTAINERS_DIR}${README_NAME}"

# page <dir-prefix> [part...]: build a README path under a directory prefix.
page() {
  local dir="$1"
  shift
  if (($# == 0)); then
    printf '%s%s' "$dir" "$README_NAME"
  else
    local IFS=/
    printf '%s%s%s' "$dir" "$*" "$README_SUFFIX"
  fi
}

# Extra (non-folder) tags per page, space separated.
declare -A EXTRA
# extra <dir-prefix> "<tags>" [part...]
extra() {
  local dir="$1" tags="$2"
  shift 2
  EXTRA["$(page "$dir" "$@")"]="$tags"
}

EXTRA["$WIKI_README"]="index"
EXTRA["$LINUX_README"]="index"
extra "$ACCOUNTS_DIR" "identity security"
extra "$ACCOUNTS_DIR" "identity security uid" root
extra "$ACCOUNTS_DIR" "identity uid passwd" users
extra "$ACCOUNTS_DIR" "identity gid" groups
extra "$ACCOUNTS_DIR" "identity security privileges" sudo
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui"
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" budgie
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" cinnamon
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui wayland" cosmic
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" deepin
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui wayland" gnome
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui wayland kde" kde-plasma
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" lxqt
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" mate
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" pantheon
extra "$DESKTOP_ENVIRONMENTS_DIR" "desktop gui" xfce
extra "$DISPLAY_MANAGERS_DIR" "dm gui"
extra "$DISPLAY_MANAGERS_DIR" "dm gui" gdm
extra "$DISPLAY_MANAGERS_DIR" "dm gui wayland" greetd
extra "$DISPLAY_MANAGERS_DIR" "dm gui" lightdm
extra "$DISPLAY_MANAGERS_DIR" "dm gui kde" sddm
extra "$DISPLAY_SERVERS_DIR" "x11 wayland gui"
extra "$DISTROS_DIR" "distro"
extra "$DISTROS_DIR" "distro pacman rolling" arch
extra "$DISTROS_DIR" "distro rpm dnf" fedora
extra "$DISTROS_DIR" "distro rpm enterprise" fedora rhel
extra "$DISTROS_DIR" "distro deb apt" ubuntu
extra "$DISTROS_DIR" "distro deb apt upstream" ubuntu debian
extra "$DISTROS_DIR" "distro" others
extra "$DISTROS_DIR" "distro musl apk" others alpine
extra "$DISTROS_DIR" "distro source portage" others gentoo
extra "$DISTROS_DIR" "distro declarative nix" others nixos
extra "$DISTROS_DIR" "distro rpm zypper" others opensuse
extra "$DISTROS_DIR" "distro" others slackware
extra "$DISTROS_DIR" "distro xbps runit" others void
extra "$DISTROS_DIR" "distro immutable" others specialty
extra "$LINUX_KERNEL_DIR" "kernel"
extra "$LINUX_KERNEL_DIR" "kernel bootloader initramfs" boot
extra "$LINUX_KERNEL_DIR" "kernel hardware" firmware
extra "$LINUX_KERNEL_DIR" "kernel drivers" modules
extra "$LINUX_KERNEL_DIR" "kernel tuning" sysctl
extra "$NETWORKING_DIR" "network ip dns"
extra "$PACKAGE_MANAGERS_DIR" "packages apt dnf pacman"
extra "$RESOURCES_DIR" "ops performance"
extra "$RESOURCES_DIR" "ops performance cgroups" cpu
extra "$RESOURCES_DIR" "ops storage filesystem" disk
extra "$RESOURCES_DIR" "ops ulimit cgroups" limits
extra "$RESOURCES_DIR" "ops performance oom" memory
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell terminal"
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell" shells
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell" shells bash
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell posix" shells dash
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell" shells fish
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell" shells nushell
extra "$SHELLS_AND_TERMINALS_DIR" "cli shell" shells zsh
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal" terminals
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal gpu" terminals alacritty
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal wayland" terminals foot
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal gtk" terminals gnome-terminal
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal gpu" terminals kitty
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal kde" terminals konsole
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal gpu" terminals wezterm
extra "$SHELLS_AND_TERMINALS_DIR" "cli terminal x11" terminals xterm
extra "$SHELLS_AND_TERMINALS_DIR" "cli multiplexer" multiplexers
extra "$SHELLS_AND_TERMINALS_DIR" "cli multiplexer" multiplexers tmux
extra "$SHELLS_AND_TERMINALS_DIR" "cli multiplexer" multiplexers zellij
extra "$SYSTEMD_DIR" "init services units"
extra "$WINDOW_MANAGERS_DIR" "wm gui"
extra "$WINDOW_MANAGERS_DIR" "wm tiling x11" bspwm
extra "$WINDOW_MANAGERS_DIR" "wm tiling wayland" hyprland
extra "$WINDOW_MANAGERS_DIR" "wm tiling x11" i3
extra "$WINDOW_MANAGERS_DIR" "wm stacking x11" openbox
extra "$WINDOW_MANAGERS_DIR" "wm tiling wayland" sway
EXTRA["$CONTAINERS_README"]="index oci"
extra "$DOCKER_DIR" "oci engine"
extra "$DOCKER_DIR" "oci install" platforms
extra "$DOCKER_DIR" "oci install engine" platforms linux
extra "$DOCKER_DIR" "oci install desktop wsl" platforms windows
extra "$DOCKER_DIR" "oci install desktop vm" platforms macos
extra "$DOCKER_DIR" "oci build images" dockerfile
extra "$DOCKER_DIR" "oci images registry" images
extra "$DOCKER_DIR" "oci lifecycle" containers
extra "$DOCKER_DIR" "oci storage persistence" volumes
extra "$DOCKER_DIR" "oci network" networking
extra "$DOCKER_DIR" "oci security" security
extra "$DOCKER_DIR" "oci cli" cli
extra "$DOCKER_DIR" "oci cli compose orchestration" cli compose
extra "$DOCKER_DIR" "oci cli debug" cli exec
extra "$DOCKER_DIR" "oci cli ops" cli logging
extra "$DOCKER_DIR" "oci cli build buildkit" cli build
extra "$DOCKER_DIR" "oci cli build" cli dockerignore
extra "$DOCKER_DIR" "oci cli ops reliability" cli healthchecks
extra "$DOCKER_DIR" "oci cli remote" cli contexts
extra "$DOCKER_DIR" "oci dev vscode" devcontainers
extra "$DOCKER_DIR" "oci debug ops" troubleshooting
extra "$PODMAN_DIR" "oci engine daemonless"
extra "$PODMAN_DIR" "oci security rootless" rootless
extra "$PODMAN_DIR" "oci desktop gui" podman-desktop
extra "$PODMAN_DIR" "oci compose orchestration" compose
extra "$RUNTIMES_DIR" "oci runtime"
extra "$RUNTIMES_DIR" "oci runtime cri" containerd
extra "$RUNTIMES_DIR" "oci runtime cri kubernetes" cri-o
extra "$IMAGE_BUILDERS_DIR" "oci build"
extra "$IMAGE_BUILDERS_DIR" "oci build daemonless" buildah
extra "$IMAGE_BUILDERS_DIR" "oci build ci kubernetes" kaniko
extra "$DESKTOP_INTERFACES_DIR" "oci desktop gui"
extra "$DESKTOP_INTERFACES_DIR" "oci desktop vm macos linux" colima
extra "$DESKTOP_INTERFACES_DIR" "oci desktop vm macos" orbstack
extra "$DESKTOP_INTERFACES_DIR" "oci desktop kubernetes" rancher-desktop

# tags_for <rel>: folder tags plus extras, unique, sorted case-insensitively.
tags_for() {
  local rel="$1" folder
  folder="${rel%"$README_SUFFIX"}"
  if [[ "$rel" == "$README_NAME" ]]; then
    folder="wiki"
  fi
  {
    tr '/' '\n' <<<"$folder"
    tr ' ' '\n' <<<"${EXTRA[$rel]:-}"
  } | sed '/^$/d' | sort -f -u | paste -sd' '
}

# rel_folder <rel>: path under wiki/ without the trailing /README.md.
rel_folder() {
  local rel="$1"
  if [[ "$rel" == "$README_NAME" ]]; then
    printf ''
  else
    printf '%s' "${rel%"$README_SUFFIX"}"
  fi
}

# set_tags <file> <tags>: drop every Tags: line, then put one under the first H1.
set_tags() {
  awk -v tags="$2" '
    /^Tags:/ { $0 = "" }
    { lines[++n] = $0 }
    END {
      # collapse runs of blank lines to one
      for (i = 1; i <= n; i++) {
        if (lines[i] == "" && m > 0 && out[m] == "") continue
        out[++m] = lines[i]
      }
      h = 0
      for (i = 1; i <= m; i++) if (out[i] ~ /^# [^ \t]/) { h = i; break }
      if (!h) exit 3
      for (i = 1; i <= h; i++) print out[i]
      line = "Tags:"
      split(tags, t, " ")
      for (i = 1; i in t; i++) line = line " `" t[i] "`"
      print ""
      print line
      print ""
      i = h + 1
      while (i <= m && out[i] == "") i++
      for (; i <= m; i++) print out[i]
    }
  ' "$1"
}

title_of() {
  awk '/^# [^ \t]/ { sub(/^# /, ""); gsub(/^[ \t]+|[ \t]+$/, ""); print; found = 1; exit } END { if (!found) print "Untitled" }' "$1"
}

backtick_tags() {
  local t out=""
  for t in $1; do out+="${out:+ }\`$t\`"; done
  printf '%s' "$out"
}

# Resolve scope args to absolute paths (empty means everything).
SCOPES=()
for arg in "$@"; do
  if [[ -d "$arg" ]]; then
    SCOPES+=("$(CDPATH= cd "$arg" && pwd)")
  elif [[ -f "$arg" ]]; then
    SCOPES+=("$(CDPATH= cd "$(dirname "$arg")" && pwd)/$(basename "$arg")")
  else
    echo "Not found: $arg" >&2
    exit 1
  fi
done

in_scope() {
  local p="$1" s
  ((${#SCOPES[@]} == 0)) && return 0
  for s in "${SCOPES[@]}"; do
    [[ "$p" == "$s" || "$p" == "$s"/* ]] && return 0
  done
  return 1
}

# Page records, one per line: sortkey TAB rel TAB tags TAB title
RECORDS=""
changed=0
total=0

while IFS= read -r -d '' path; do
  rel="${path#"$ROOT"/}"
  tags="$(tags_for "$rel")"
  if ! grep -qE '^# [^[:space:]]' "$path"; then
    echo "skipped (no H1): $rel"
    continue
  fi
  updated="$(set_tags "$path" "$tags"; echo x)"
  updated="${updated%x}"
  original="$(cat "$path"; echo x)"
  original="${original%x}"
  if in_scope "$path" && [[ "$updated" != "$original" ]]; then
    printf '%s' "$updated" >"$path"
    changed=$((changed + 1))
  fi
  total=$((total + 1))
  folder="$(rel_folder "$rel")"
  key="0${folder//\//$'\x01'}"
  RECORDS+="${key}"$'\t'"${rel}"$'\t'"${tags}"$'\t'"$(title_of "$path")"$'\n'
done < <(find "$ROOT" \( -name .git -o -name graft -o -name node_modules -o -name tmp \) -prune -o -type f -name "$README_NAME" -print0 | sort -z)

RECORDS="$(printf '%s' "$RECORDS" | sort -t $'\t' -k1,1)"

toc=""
add() { toc+="$1"$'\n'; }

# emit_section <heading> <prefix>
emit_section() {
  local heading="$1" prefix="$2" base base_folder key rel tags title folder rest depth pad i
  base="${prefix%"$README_SUFFIX"}"
  base_folder="$base"
  add "### $heading"
  add ""
  while IFS=$'\t' read -r key rel tags title; do
    [[ "$rel" == "$prefix" || "$rel" == "$base/"* ]] || continue
    folder="$(rel_folder "$rel")"
    if [[ "$folder" == "$base_folder" ]]; then
      depth=0
    else
      rest="${folder:${#base_folder}}"
      rest="${rest#/}"
      depth=$(awk -F/ '{ print NF }' <<<"$rest")
    fi
    pad=""
    for ((i = 0; i < depth; i++)); do pad+="  "; done
    add "${pad}- [$title]($rel) - $(backtick_tags "$tags")"
  done <<<"$RECORDS"
  add ""
}

add "$TOC_START"
add ""
add 'Generated by `scripts/sync-tags.sh`. Do not hand-edit between the markers.'
add ""
add 'Each page has a `Tags:` line under the title. Path folders become tags; pages may add a few semantic tags.'
add ""

emit_section "Linux" "$LINUX_README"
emit_section "Containers" "$CONTAINERS_README"

add "### By tag"
add ""
while IFS= read -r tag; do
  add "#### \`$tag\`"
  add ""
  while IFS=$'\t' read -r _ rel title; do
    add "- [$title]($rel)"
  done < <(
    while IFS=$'\t' read -r _ rel tags title; do
      case " $tags " in
        *" $tag "*) printf '%s\t%s\t%s\n' "${title,,}" "$rel" "$title" ;;
      esac
    done <<<"$RECORDS" | sort -t $'\t' -k1,1 -k2,2
  )
  add ""
done < <(
  while IFS=$'\t' read -r _ _ tags _; do tr ' ' '\n' <<<"$tags"; done <<<"$RECORDS" | sed '/^$/d' | sort -f -u
)

toc+="$TOC_END"

readme="$ROOT/$README_NAME"
text="$(cat "$readme"; echo x)"
text="${text%x}"
case "$text" in
  *"$TOC_START"*"$TOC_END"*) ;;
  *)
    echo "$README_NAME is missing the $TOC_START / $TOC_END markers" >&2
    exit 1
    ;;
esac
head_part="${text%%"$TOC_START"*}"
tail_part="${text#*"$TOC_END"}"
printf '%s%s%s' "$head_part" "$toc" "$tail_part" >"$readme"

echo "Updated tags on $changed / $total pages"
echo "Rewrote table of contents in $README_NAME"
