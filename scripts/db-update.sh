#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/db-common.sh"

PHASE="${1:-}"
if [[ -z "$PHASE" ]]; then
  echo "Usage: scripts/db-update.sh <bootstrap|migrate|seed>" >&2
  exit 1
fi

case "$PHASE" in
  bootstrap)
    LOG_FILE="$LOG_DIR/${RUN_TS}-bootstrap.log"
    CHANGELOG="$REPO_ROOT/db/liquibase/bootstrap-changelog.xml"
    TITLE="Liquibase bootstrap changelog"
    SUCCESS="Bootstrap complete. Log: $LOG_FILE"
    RUNNER="admin"
    ;;
  migrate)
    LOG_FILE="$LOG_DIR/${RUN_TS}-migrate.log"
    CHANGELOG="$REPO_ROOT/db/liquibase/app-migrations-changelog.xml"
    TITLE="Liquibase migration changelog"
    SUCCESS="Migrations complete. Log: $LOG_FILE"
    RUNNER="app"
    ;;
  seed)
    LOG_FILE="$LOG_DIR/${RUN_TS}-seed.log"
    CHANGELOG="$REPO_ROOT/db/liquibase/app-seed-changelog.xml"
    TITLE="Liquibase seed changelog"
    SUCCESS="Seeds complete. Log: $LOG_FILE"
    RUNNER="app"
    ;;
  *)
    echo "Unknown phase: $PHASE" >&2
    echo "Usage: scripts/db-update.sh <bootstrap|migrate|seed>" >&2
    exit 1
    ;;
esac

echo "==> $TITLE" | tee -a "$LOG_FILE"
if [[ "$RUNNER" == "admin" ]]; then
  run_liquibase_admin "$CHANGELOG" update | tee -a "$LOG_FILE"
else
  run_liquibase_app "$CHANGELOG" update | tee -a "$LOG_FILE"
fi

echo "$SUCCESS"
