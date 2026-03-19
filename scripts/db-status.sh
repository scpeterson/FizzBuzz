#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/db-common.sh"

echo "==> Liquibase bootstrap history ($DB_ADMIN_DB)"
run_liquibase_admin "$REPO_ROOT/db/liquibase/bootstrap-changelog.xml" history || true

echo
echo "==> Liquibase migration history ($DB_NAME)"
run_liquibase_app "$REPO_ROOT/db/liquibase/app-migrations-changelog.xml" history || true

echo
echo "==> Liquibase seed history ($DB_NAME)"
run_liquibase_app "$REPO_ROOT/db/liquibase/app-seed-changelog.xml" history || true
