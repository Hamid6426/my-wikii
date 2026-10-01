#!/usr/bin/env bash
# Serve the repo root so index.html can fetch files from wiki/.
# Open http://localhost:8000/
set -euo pipefail
cd "$(dirname "$0")/.."
exec python3 -m http.server "${1:-8000}"
