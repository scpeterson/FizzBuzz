#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

export DB_HOST="${DB_HOST:-localhost}"
export DB_PORT="${DB_PORT:-5432}"
export DB_ADMIN_USER="${DB_ADMIN_USER:-postgres}"
export DB_ADMIN_PASSWORD="${DB_ADMIN_PASSWORD:-}"
export DB_ADMIN_DB="${DB_ADMIN_DB:-postgres}"
export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_app}"
export DB_APP_PASSWORD="${DB_APP_PASSWORD:-functional_programming_triads_app_pw}"
export DB_NAME="${DB_NAME:-functional_programming_triads_demo}"
export DB_APP_SCHEMA="${DB_APP_SCHEMA:-public}"

export PSQL_ADMIN="${PSQL_ADMIN:-psql}"
export PSQL_APP="${PSQL_APP:-psql}"
LIQUIBASE_CMD="${LIQUIBASE_CMD:-liquibase}"
LIQUIBASE_JDBC_CLASSPATH="${LIQUIBASE_JDBC_CLASSPATH:-$REPO_ROOT/tools/liquibase/postgresql-42.7.9.jar}"
LIQUIBASE_BASE_DEFAULTS_FILE="${LIQUIBASE_BASE_DEFAULTS_FILE:-}"

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

ADMIN_JDBC_URL="jdbc:postgresql://${DB_HOST}:${DB_PORT}/${DB_ADMIN_DB}"
APP_JDBC_URL="jdbc:postgresql://${DB_HOST}:${DB_PORT}/${DB_NAME}"

run_psql_admin() {
  if [[ -n "$DB_ADMIN_PASSWORD" ]]; then
    PGPASSWORD="$DB_ADMIN_PASSWORD" "$PSQL_ADMIN" "${ADMIN_CONN[@]}" "$@"
  else
    "$PSQL_ADMIN" "${ADMIN_CONN[@]}" "$@"
  fi
}

run_psql_app() {
  PGPASSWORD="$DB_APP_PASSWORD" "$PSQL_APP" "${APP_CONN[@]}" "$@"
}

ensure_liquibase_driver() {
  if [[ ! -f "$LIQUIBASE_JDBC_CLASSPATH" ]]; then
    echo "Liquibase PostgreSQL driver not found: $LIQUIBASE_JDBC_CLASSPATH" >&2
    exit 1
  fi
}

normalize_changelog_path() {
  local changelog="$1"
  if [[ "$changelog" == "$REPO_ROOT"/* ]]; then
    printf '%s\n' "${changelog#"$REPO_ROOT/"}"
  else
    printf '%s\n' "$changelog"
  fi
}

write_liquibase_defaults_file() {
  local jdbc_url="$1"
  local username="$2"
  local password="$3"
  local changelog="$4"
  local default_schema="$5"
  local defaults_file

  defaults_file="$(mktemp "$REPO_ROOT/tmp/liquibase.XXXXXX")"
  changelog="$(normalize_changelog_path "$changelog")"

  if [[ -n "$LIQUIBASE_BASE_DEFAULTS_FILE" && -f "$LIQUIBASE_BASE_DEFAULTS_FILE" ]]; then
    cat "$LIQUIBASE_BASE_DEFAULTS_FILE" > "$defaults_file"
    printf "\n" >> "$defaults_file"
  else
    : > "$defaults_file"
  fi

  cat >> "$defaults_file" <<PROPS
url=$jdbc_url
username=$username
changelog-file=$changelog
search-path=$REPO_ROOT
classpath=$LIQUIBASE_JDBC_CLASSPATH
driver=org.postgresql.Driver
default-schema-name=$default_schema
appUser=$DB_APP_USER
appPassword=$DB_APP_PASSWORD
dbName=$DB_NAME
PROPS

  if [[ -n "$password" ]]; then
    printf 'password=%s\n' "$password" >> "$defaults_file"
  fi

  printf '%s\n' "$defaults_file"
}

run_liquibase_admin() {
  local changelog="$1"
  shift
  ensure_liquibase_driver

  mkdir -p "$REPO_ROOT/tmp"
  local defaults_file
  defaults_file="$(write_liquibase_defaults_file "$ADMIN_JDBC_URL" "$DB_ADMIN_USER" "$DB_ADMIN_PASSWORD" "$changelog" "public")"
  trap 'rm -f "$defaults_file"' RETURN

  "$LIQUIBASE_CMD" --defaults-file="$defaults_file" "$@"
}

run_liquibase_app() {
  local changelog="$1"
  shift
  ensure_liquibase_driver

  mkdir -p "$REPO_ROOT/tmp"
  local defaults_file
  defaults_file="$(write_liquibase_defaults_file "$APP_JDBC_URL" "$DB_APP_USER" "$DB_APP_PASSWORD" "$changelog" "$DB_APP_SCHEMA")"
  trap 'rm -f "$defaults_file"' RETURN

  "$LIQUIBASE_CMD" --defaults-file="$defaults_file" "$@"
}
