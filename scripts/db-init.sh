#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

"$SCRIPT_DIR/db-bootstrap.sh"
"$SCRIPT_DIR/db-migrate.sh"
"$SCRIPT_DIR/db-update.sh" reference
"$SCRIPT_DIR/db-seed.sh"
"$SCRIPT_DIR/db-verify.sh"
