#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

export DB_HOST="${DB_HOST:-localhost}"
export DB_PORT="${DB_PORT:-5432}"
export DB_ADMIN_USER="${DB_ADMIN_USER:-postgres}"
export DB_ADMIN_DB="${DB_ADMIN_DB:-postgres}"
export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_app}"
export DB_APP_PASSWORD="${DB_APP_PASSWORD:-functional_programming_triads_app_pw}"
export DB_NAME="${DB_NAME:-functional_programming_triads_demo}"
export DB_APP_SCHEMA="${DB_APP_SCHEMA:-public}"

export PSQL_ADMIN="${PSQL_ADMIN:-psql}"
export PSQL_APP="${PSQL_APP:-psql}"

LOG_DIR="$REPO_ROOT/output/db-changelog"
mkdir -p "$LOG_DIR"
RUN_TS="$(date +%Y%m%d-%H%M%S)"

ADMIN_CONN=(
  -h "$DB_HOST"
  -p "$DB_PORT"
  -U "$DB_ADMIN_USER"
  -d "$DB_ADMIN_DB"
  -v ON_ERROR_STOP=1
)

APP_CONN=(
  -h "$DB_HOST"
  -p "$DB_PORT"
  -U "$DB_APP_USER"
  -d "$DB_NAME"
  -v ON_ERROR_STOP=1
)

checksum_file() {
  local file="$1"
  shasum -a 256 "$file" | awk '{print $1}'
}

run_psql_admin() {
  "$PSQL_ADMIN" "${ADMIN_CONN[@]}" "$@"
}

run_psql_app() {
  "$PSQL_APP" "${APP_CONN[@]}" "$@"
}
