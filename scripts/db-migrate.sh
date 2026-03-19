#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/db-common.sh"

LOG_FILE="$LOG_DIR/${RUN_TS}-migrate.log"
CHANGELOG="$REPO_ROOT/db/liquibase/app-migrations-changelog.xml"

echo "==> Liquibase migration changelog" | tee -a "$LOG_FILE"
run_liquibase_app "$CHANGELOG" update | tee -a "$LOG_FILE"

echo "Migrations complete. Log: $LOG_FILE"
