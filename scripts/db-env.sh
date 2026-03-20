#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

ENV_NAME="${1:-}"
ACTION="${2:-}"

if [[ -z "$ENV_NAME" || -z "$ACTION" ]]; then
  echo "Usage: scripts/db-env.sh <dev|qa|stage|prod> <init|bootstrap|migrate|reference|seed|verify|status|validate|update>" >&2
  exit 1
fi

case "$ENV_NAME" in
  dev|qa|stage|prod) ;;
  *)
    echo "Unknown environment: $ENV_NAME" >&2
    exit 1
    ;;
esac

export LIQUIBASE_BASE_DEFAULTS_FILE="$REPO_ROOT/config/liquibase/liquibase-${ENV_NAME}.properties"

case "$ENV_NAME" in
  dev)
    export DB_NAME="${DB_NAME:-functional_programming_triads_dev}"
    export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_dev_app}"
    export DB_APP_PASSWORD="${DB_APP_PASSWORD:-functional_programming_triads_dev_app_pw}"
    ;;
  qa)
    export DB_NAME="${DB_NAME:-functional_programming_triads_qa}"
    export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_qa_app}"
    export DB_APP_PASSWORD="${DB_APP_PASSWORD:-${FPT_DB_APP_PASSWORD_QA:-}}"
    ;;
  stage)
    export DB_NAME="${DB_NAME:-functional_programming_triads_stage}"
    export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_stage_app}"
    export DB_APP_PASSWORD="${DB_APP_PASSWORD:-${FPT_DB_APP_PASSWORD_STAGE:-}}"
    ;;
  prod)
    export DB_NAME="${DB_NAME:-functional_programming_triads_prod}"
    export DB_APP_USER="${DB_APP_USER:-functional_programming_triads_prod_app}"
    export DB_APP_PASSWORD="${DB_APP_PASSWORD:-${FPT_DB_APP_PASSWORD_PROD:-}}"
    ;;
esac

case "$ACTION" in
  seed|demo-seed)
    if [[ "$ENV_NAME" == "stage" || "$ENV_NAME" == "prod" ]]; then
      echo "Refusing to run demo seed in $ENV_NAME." >&2
      exit 1
    fi
    ;;
  bootstrap)
    if [[ "$ENV_NAME" == "prod" ]]; then
      echo "Refusing to run bootstrap directly in prod." >&2
      exit 1
    fi
    ;;
esac

if [[ "$ENV_NAME" != "dev" && -z "${DB_APP_PASSWORD:-}" && "$ACTION" != "status" && "$ACTION" != "validate" ]]; then
  echo "DB_APP_PASSWORD is required for $ENV_NAME $ACTION. Set DB_APP_PASSWORD or the matching FPT_DB_APP_PASSWORD_* variable." >&2
  exit 1
fi

run_validate() {
  source "$SCRIPT_DIR/db-common.sh"
  run_liquibase_admin "$REPO_ROOT/db/liquibase/bootstrap-changelog.xml" validate
  run_liquibase_app "$REPO_ROOT/db/liquibase/app-migrations-changelog.xml" validate
  run_liquibase_app "$REPO_ROOT/db/liquibase/app-reference-data-changelog.xml" validate
  if [[ "$ENV_NAME" == "dev" || "$ENV_NAME" == "qa" ]]; then
    run_liquibase_app "$REPO_ROOT/db/liquibase/app-demo-seed-changelog.xml" validate
  fi
}

case "$ACTION" in
  init)
    "$SCRIPT_DIR/db-update.sh" bootstrap
    "$SCRIPT_DIR/db-update.sh" migrate
    "$SCRIPT_DIR/db-update.sh" reference
    if [[ "$ENV_NAME" == "dev" || "$ENV_NAME" == "qa" ]]; then
      "$SCRIPT_DIR/db-update.sh" seed
    fi
    "$SCRIPT_DIR/db-verify.sh"
    ;;
  bootstrap)
    "$SCRIPT_DIR/db-update.sh" bootstrap
    ;;
  migrate)
    "$SCRIPT_DIR/db-update.sh" migrate
    ;;
  reference)
    "$SCRIPT_DIR/db-update.sh" reference
    ;;
  seed|demo-seed)
    "$SCRIPT_DIR/db-update.sh" seed
    ;;
  verify)
    "$SCRIPT_DIR/db-verify.sh"
    ;;
  status)
    "$SCRIPT_DIR/db-status.sh"
    ;;
  validate)
    run_validate
    ;;
  update)
    "$SCRIPT_DIR/db-update.sh" migrate
    "$SCRIPT_DIR/db-update.sh" reference
    if [[ "$ENV_NAME" == "dev" || "$ENV_NAME" == "qa" ]]; then
      "$SCRIPT_DIR/db-update.sh" seed
    fi
    ;;
  *)
    echo "Unknown action: $ACTION" >&2
    exit 1
    ;;
esac
