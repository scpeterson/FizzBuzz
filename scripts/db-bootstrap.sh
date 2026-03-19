#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/db-common.sh"

LOG_FILE="$LOG_DIR/${RUN_TS}-bootstrap.log"
CHANGELOG="$REPO_ROOT/db/liquibase/bootstrap-changelog.xml"

echo "==> Liquibase bootstrap changelog" | tee -a "$LOG_FILE"
run_liquibase_admin "$CHANGELOG" update | tee -a "$LOG_FILE"

echo "Bootstrap complete. Log: $LOG_FILE"
