#!/usr/bin/env python3
"""Add Tags: lines to wiki pages and regenerate the table of contents inside wiki/README.md."""
from __future__ import annotations

import re
from collections import defaultdict
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "wiki"
WIKI_PREFIX = ""
README_NAME = "README.md"
README_SUFFIX = f"/{README_NAME}"
TOC_START = "<!-- toc:start -->"
TOC_END = "<!-- toc:end -->"

# Level-2 directory prefixes (trailing slash).
LINUX_DIR = f"{WIKI_PREFIX}linux/"
CONTAINERS_DIR = f"{WIKI_PREFIX}containers/"

# Level-3 directory prefixes under linux/
ACCOUNTS_DIR = f"{LINUX_DIR}accounts/"
DESKTOP_ENVIRONMENTS_DIR = f"{LINUX_DIR}desktop-environments/"
DISPLAY_MANAGERS_DIR = f"{LINUX_DIR}display-managers/"
DISPLAY_SERVERS_DIR = f"{LINUX_DIR}display-servers/"
DISTROS_DIR = f"{LINUX_DIR}distros/"
LINUX_KERNEL_DIR = f"{LINUX_DIR}linux-kernel/"
NETWORKING_DIR = f"{LINUX_DIR}networking/"
PACKAGE_MANAGERS_DIR = f"{LINUX_DIR}package-managers/"
RESOURCES_DIR = f"{LINUX_DIR}resources/"
SHELLS_AND_TERMINALS_DIR = f"{LINUX_DIR}shells-and-terminals/"
SYSTEMD_DIR = f"{LINUX_DIR}systemd/"
WINDOW_MANAGERS_DIR = f"{LINUX_DIR}window-managers/"

# Level-3 directory prefixes under containers/.
DOCKER_DIR = f"{CONTAINERS_DIR}docker/"
PODMAN_DIR = f"{CONTAINERS_DIR}podman/"
RUNTIMES_DIR = f"{CONTAINERS_DIR}runtimes/"
IMAGE_BUILDERS_DIR = f"{CONTAINERS_DIR}image-builders/"
DESKTOP_INTERFACES_DIR = f"{CONTAINERS_DIR}desktop-interfaces/"

# Hub page paths
WIKI_README = f"{WIKI_PREFIX}{README_NAME}"
LINUX_README = f"{LINUX_DIR}{README_NAME}"
CONTAINERS_README = f"{CONTAINERS_DIR}{README_NAME}"


def page(dir_prefix: str, *parts: str) -> str:
    """Build `wiki/.../README.md` under a directory prefix."""
    if not parts:
        return f"{dir_prefix}{README_NAME}"
    return f"{dir_prefix}{'/'.join(parts)}{README_SUFFIX}"


EXTRA: dict[str, list[str]] = {
    WIKI_README: ["index"],
    LINUX_README: ["index"],
    page(ACCOUNTS_DIR): ["identity", "security"],
    page(ACCOUNTS_DIR, "root"): ["identity", "security", "uid"],
    page(ACCOUNTS_DIR, "users"): ["identity", "uid", "passwd"],
    page(ACCOUNTS_DIR, "groups"): ["identity", "gid"],
    page(ACCOUNTS_DIR, "sudo"): ["identity", "security", "privileges"],
    page(DESKTOP_ENVIRONMENTS_DIR): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "budgie"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "cinnamon"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "cosmic"): ["desktop", "gui", "wayland"],
    page(DESKTOP_ENVIRONMENTS_DIR, "deepin"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "gnome"): ["desktop", "gui", "wayland"],
    page(DESKTOP_ENVIRONMENTS_DIR, "kde-plasma"): ["desktop", "gui", "wayland", "kde"],
    page(DESKTOP_ENVIRONMENTS_DIR, "lxqt"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "mate"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "pantheon"): ["desktop", "gui"],
    page(DESKTOP_ENVIRONMENTS_DIR, "xfce"): ["desktop", "gui"],
    page(DISPLAY_MANAGERS_DIR): ["dm", "gui"],
    page(DISPLAY_MANAGERS_DIR, "gdm"): ["dm", "gui"],
    page(DISPLAY_MANAGERS_DIR, "greetd"): ["dm", "gui", "wayland"],
    page(DISPLAY_MANAGERS_DIR, "lightdm"): ["dm", "gui"],
    page(DISPLAY_MANAGERS_DIR, "sddm"): ["dm", "gui", "kde"],
    page(DISPLAY_SERVERS_DIR): ["x11", "wayland", "gui"],
    page(DISTROS_DIR): ["distro"],
    page(DISTROS_DIR, "arch"): ["distro", "pacman", "rolling"],
    page(DISTROS_DIR, "fedora"): ["distro", "rpm", "dnf"],
    page(DISTROS_DIR, "fedora", "rhel"): ["distro", "rpm", "enterprise"],
    page(DISTROS_DIR, "ubuntu"): ["distro", "deb", "apt"],
    page(DISTROS_DIR, "ubuntu", "debian"): ["distro", "deb", "apt", "upstream"],
    page(DISTROS_DIR, "others"): ["distro"],
    page(DISTROS_DIR, "others", "alpine"): ["distro", "musl", "apk"],
    page(DISTROS_DIR, "others", "gentoo"): ["distro", "source", "portage"],
    page(DISTROS_DIR, "others", "nixos"): ["distro", "declarative", "nix"],
    page(DISTROS_DIR, "others", "opensuse"): ["distro", "rpm", "zypper"],
    page(DISTROS_DIR, "others", "slackware"): ["distro"],
    page(DISTROS_DIR, "others", "void"): ["distro", "xbps", "runit"],
    page(DISTROS_DIR, "others", "specialty"): ["distro", "immutable"],
    page(LINUX_KERNEL_DIR): ["kernel"],
    page(LINUX_KERNEL_DIR, "boot"): ["kernel", "bootloader", "initramfs"],
    page(LINUX_KERNEL_DIR, "firmware"): ["kernel", "hardware"],
    page(LINUX_KERNEL_DIR, "modules"): ["kernel", "drivers"],
    page(LINUX_KERNEL_DIR, "sysctl"): ["kernel", "tuning"],
    page(NETWORKING_DIR): ["network", "ip", "dns"],
    page(PACKAGE_MANAGERS_DIR): ["packages", "apt", "dnf", "pacman"],
    page(RESOURCES_DIR): ["ops", "performance"],
    page(RESOURCES_DIR, "cpu"): ["ops", "performance", "cgroups"],
    page(RESOURCES_DIR, "disk"): ["ops", "storage", "filesystem"],
    page(RESOURCES_DIR, "limits"): ["ops", "ulimit", "cgroups"],
    page(RESOURCES_DIR, "memory"): ["ops", "performance", "oom"],
    page(SHELLS_AND_TERMINALS_DIR): ["cli", "shell", "terminal"],
    page(SHELLS_AND_TERMINALS_DIR, "shells"): ["cli", "shell"],
    page(SHELLS_AND_TERMINALS_DIR, "shells", "bash"): ["cli", "shell"],
    page(SHELLS_AND_TERMINALS_DIR, "shells", "dash"): ["cli", "shell", "posix"],
    page(SHELLS_AND_TERMINALS_DIR, "shells", "fish"): ["cli", "shell"],
    page(SHELLS_AND_TERMINALS_DIR, "shells", "nushell"): ["cli", "shell"],
    page(SHELLS_AND_TERMINALS_DIR, "shells", "zsh"): ["cli", "shell"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals"): ["cli", "terminal"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "alacritty"): ["cli", "terminal", "gpu"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "foot"): ["cli", "terminal", "wayland"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "gnome-terminal"): ["cli", "terminal", "gtk"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "kitty"): ["cli", "terminal", "gpu"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "konsole"): ["cli", "terminal", "kde"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "wezterm"): ["cli", "terminal", "gpu"],
    page(SHELLS_AND_TERMINALS_DIR, "terminals", "xterm"): ["cli", "terminal", "x11"],
    page(SHELLS_AND_TERMINALS_DIR, "multiplexers"): ["cli", "multiplexer"],
    page(SHELLS_AND_TERMINALS_DIR, "multiplexers", "tmux"): ["cli", "multiplexer"],
    page(SHELLS_AND_TERMINALS_DIR, "multiplexers", "zellij"): ["cli", "multiplexer"],
    page(SYSTEMD_DIR): ["init", "services", "units"],
    page(WINDOW_MANAGERS_DIR): ["wm", "gui"],
    page(WINDOW_MANAGERS_DIR, "bspwm"): ["wm", "tiling", "x11"],
    page(WINDOW_MANAGERS_DIR, "hyprland"): ["wm", "tiling", "wayland"],
    page(WINDOW_MANAGERS_DIR, "i3"): ["wm", "tiling", "x11"],
    page(WINDOW_MANAGERS_DIR, "openbox"): ["wm", "stacking", "x11"],
    page(WINDOW_MANAGERS_DIR, "sway"): ["wm", "tiling", "wayland"],
    CONTAINERS_README: ["index", "oci"],
    page(DOCKER_DIR): ["oci", "engine"],
    page(DOCKER_DIR, "platforms"): ["oci", "install"],
    page(DOCKER_DIR, "platforms", "linux"): ["oci", "install", "engine"],
    page(DOCKER_DIR, "platforms", "windows"): ["oci", "install", "desktop", "wsl"],
    page(DOCKER_DIR, "platforms", "macos"): ["oci", "install", "desktop", "vm"],
    page(DOCKER_DIR, "dockerfile"): ["oci", "build", "images"],
    page(DOCKER_DIR, "images"): ["oci", "images", "registry"],
    page(DOCKER_DIR, "containers"): ["oci", "lifecycle"],
    page(DOCKER_DIR, "volumes"): ["oci", "storage", "persistence"],
    page(DOCKER_DIR, "networking"): ["oci", "network"],
    page(DOCKER_DIR, "security"): ["oci", "security"],
    page(DOCKER_DIR, "cli"): ["oci", "cli"],
    page(DOCKER_DIR, "cli", "compose"): ["oci", "cli", "compose", "orchestration"],
    page(DOCKER_DIR, "cli", "exec"): ["oci", "cli", "debug"],
    page(DOCKER_DIR, "cli", "logging"): ["oci", "cli", "ops"],
    page(DOCKER_DIR, "cli", "build"): ["oci", "cli", "build", "buildkit"],
    page(DOCKER_DIR, "cli", "dockerignore"): ["oci", "cli", "build"],
    page(DOCKER_DIR, "cli", "healthchecks"): ["oci", "cli", "ops", "reliability"],
    page(DOCKER_DIR, "cli", "contexts"): ["oci", "cli", "remote"],
    page(DOCKER_DIR, "devcontainers"): ["oci", "dev", "vscode"],
    page(DOCKER_DIR, "troubleshooting"): ["oci", "debug", "ops"],
    page(PODMAN_DIR): ["oci", "engine", "daemonless"],
    page(PODMAN_DIR, "rootless"): ["oci", "security", "rootless"],
    page(PODMAN_DIR, "podman-desktop"): ["oci", "desktop", "gui"],
    page(PODMAN_DIR, "compose"): ["oci", "compose", "orchestration"],
    page(RUNTIMES_DIR): ["oci", "runtime"],
    page(RUNTIMES_DIR, "containerd"): ["oci", "runtime", "cri"],
    page(RUNTIMES_DIR, "cri-o"): ["oci", "runtime", "cri", "kubernetes"],
    page(IMAGE_BUILDERS_DIR): ["oci", "build"],
    page(IMAGE_BUILDERS_DIR, "buildah"): ["oci", "build", "daemonless"],
    page(IMAGE_BUILDERS_DIR, "kaniko"): ["oci", "build", "ci", "kubernetes"],
    page(DESKTOP_INTERFACES_DIR): ["oci", "desktop", "gui"],
    page(DESKTOP_INTERFACES_DIR, "colima"): ["oci", "desktop", "vm", "macos", "linux"],
    page(DESKTOP_INTERFACES_DIR, "orbstack"): ["oci", "desktop", "vm", "macos"],
    page(DESKTOP_INTERFACES_DIR, "rancher-desktop"): ["oci", "desktop", "kubernetes"],
}

TAGS_LINE_RE = re.compile(r"^Tags:.*$", re.MULTILINE)
H1_RE = re.compile(r"^# (\S.*)$", re.MULTILINE)


def rel_folder(rel: str) -> str:
    """Path under wiki/ with trailing /README.md removed (e.g. linux/accounts)."""
    return rel.removeprefix(WIKI_PREFIX).removesuffix(README_SUFFIX)


def path_tags(rel: str) -> list[str]:
    parts = rel_folder(rel)
    if not parts or parts == README_NAME:
        return ["wiki"]
    return [p for p in parts.split("/") if p]


def tags_for(rel: str) -> list[str]:
    tags = set(path_tags(rel))
    tags.update(EXTRA.get(rel, []))
    return sorted(tags, key=str.lower)


def format_tags_line(tags: list[str]) -> str:
    return "Tags: " + " ".join(f"`{t}`" for t in tags)


def set_tags(text: str, tags: list[str]) -> str:
    # Remove every top-level Tags: line (keep accidental mentions in prose that aren't whole-line Tags:)
    text = TAGS_LINE_RE.sub("", text)
    # Collapse excess blank lines created by removals (max 2 newlines)
    text = re.sub(r"\n{3,}", "\n\n", text)
    m = H1_RE.search(text)
    if not m:
        raise ValueError("missing H1")
    insert_at = m.end()
    line = format_tags_line(tags)
    return text[:insert_at] + "\n\n" + line + "\n\n" + text[insert_at:].lstrip("\n")


def title_of(text: str) -> str:
    m = H1_RE.search(text)
    return m.group(1).strip() if m else "Untitled"


def sort_key(rel: str) -> tuple:
    # Parent README before children: wiki/linux/README.md -> ('linux',)
    # wiki/linux/accounts/README.md -> ('linux', 'accounts')
    folder = rel_folder(rel)
    if folder == README_NAME or folder == "":
        return ("",)
    return tuple(folder.split("/"))


def indent(depth: int) -> str:
    return "  " * depth


def emit_section(lines: list[str], heading: str, pages: list[dict], prefix: str) -> None:
    lines.append(f"### {heading}")
    lines.append("")
    base = prefix.removesuffix(README_SUFFIX)
    section = [p for p in pages if p["rel"] == prefix or p["rel"].startswith(base + "/")]
    section.sort(key=lambda p: sort_key(p["rel"]))
    for p in section:
        folder = rel_folder(p["rel"])
        base_folder = base.removeprefix(WIKI_PREFIX)
        if folder == base_folder:
            depth = 0
        else:
            rest = folder[len(base_folder) :].lstrip("/")
            depth = len(rest.split("/"))
        tag_bits = " ".join(f"`{t}`" for t in p["tags"])
        lines.append(f"{indent(depth)}- [{p['title']}]({p['rel']}) - {tag_bits}")
    lines.append("")


def main() -> None:
    pages: list[dict] = []
    changed = 0
    for path in sorted(ROOT.rglob(README_NAME)):
        rel = path.relative_to(ROOT).as_posix()
        tags = tags_for(rel)
        original = path.read_text(encoding="utf-8")
        if not H1_RE.search(original):
            print(f"skipped (no H1): {rel}")
            continue
        updated = set_tags(original, tags)
        if updated != original:
            path.write_text(updated, encoding="utf-8", newline="\n")
            changed += 1
        pages.append(
            {
                "rel": rel,
                "tags": tags,
                "title": title_of(updated),
                "path": path,
            }
        )

    pages.sort(key=lambda p: sort_key(p["rel"]))

    toc: list[str] = [
        TOC_START,
        "",
        "Generated by `scripts/sync-tags.py`. Do not hand-edit between the markers.",
        "",
        "Each page has a `Tags:` line under the title. Path folders become tags; pages may add a few semantic tags.",
        "",
    ]

    emit_section(toc, "Linux", pages, LINUX_README)
    emit_section(toc, "Containers", pages, CONTAINERS_README)

    by_tag: dict[str, list[dict]] = defaultdict(list)
    for p in pages:
        for t in p["tags"]:
            by_tag[t].append(p)

    toc.append("### By tag")
    toc.append("")
    for t in sorted(by_tag, key=str.lower):
        toc.append(f"#### `{t}`")
        toc.append("")
        for p in sorted(by_tag[t], key=lambda x: (x["title"].lower(), x["rel"])):
            toc.append(f"- [{p['title']}]({p['rel']})")
        toc.append("")

    toc.append(TOC_END)
    readme = ROOT / README_NAME
    text = readme.read_text(encoding="utf-8")
    start, end = text.find(TOC_START), text.find(TOC_END)
    if start == -1 or end == -1:
        raise SystemExit(f"{README_NAME} is missing the {TOC_START} / {TOC_END} markers")
    text = text[:start] + "\n".join(toc) + text[end + len(TOC_END) :]
    readme.write_text(text, encoding="utf-8", newline="\n")
    print(f"Updated tags on {changed} / {len(pages)} pages")
    print(f"Rewrote table of contents in {README_NAME}")


if __name__ == "__main__":
    main()
