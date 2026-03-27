# Database Operations Runbook

## Default Local Values

```bash
export DB_HOST=localhost
export DB_PORT=5432
export DB_ADMIN_USER=postgres
export DB_ADMIN_DB=postgres
export DB_APP_USER=functional_programming_triads_app
export DB_APP_PASSWORD=functional_programming_triads_app_pw
export DB_NAME=functional_programming_triads_demo
```

## Bootstrap + Build From Scratch

```bash
scripts/db-init.sh
```

That runs:

1. `scripts/db-update.sh bootstrap`
2. `scripts/db-update.sh migrate`
3. `scripts/db-update.sh reference`
4. `scripts/db-update.sh seed`
5. `scripts/db-verify.sh`

Compatibility note: `scripts/db-bootstrap.sh`, `scripts/db-migrate.sh`, and `scripts/db-seed.sh` still exist as wrappers, but `scripts/db-update.sh` is now the shared implementation.

## Inspect Current Liquibase State

```bash
scripts/db-status.sh
```

## Rebuild The Local Database

```bash
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

This will:

1. terminate active connections to the app database
2. drop the app database
3. drop the app role
4. clear Liquibase bootstrap tracking tables from the admin database
5. clear the legacy `admin_bootstrap_history` table if it exists
6. rerun bootstrap, migrations, seeds, and verification

## Runtime Demo Connection

```bash
export FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app_pw"
```

## Liquibase Notes

- Liquibase is used as the orchestration layer.
- The actual schema and seed work still lives primarily in SQL files under `db/`.
- The local PostgreSQL JDBC driver used by Liquibase is expected at:
  - `tools/liquibase/postgresql-42.7.9.jar`
- The JDBC driver is intentionally not committed. Install it locally with:

```bash
mkdir -p tools/liquibase
curl -L --fail --output tools/liquibase/postgresql-42.7.9.jar https://jdbc.postgresql.org/download/postgresql-42.7.9.jar
```

## Environment-Aware Commands

```bash
scripts/db-env.sh dev init
scripts/db-env.sh qa validate
scripts/db-env.sh stage update
scripts/db-env.sh prod status
```

Notes:

- `seed` is blocked in `stage` and `prod`.
- `bootstrap` is blocked in `prod`.
- non-dev environments expect `DB_APP_PASSWORD` or the matching `FPT_DB_APP_PASSWORD_*` variable.

## Run Each Environment

### Development

Use development when you want the full local workflow, including bootstrap, migrations, reference data, demo seed data, and verification.

```bash
scripts/db-env.sh dev init
```

Useful follow-up commands:

```bash
scripts/db-env.sh dev status
scripts/db-env.sh dev validate
scripts/db-env.sh dev seed
```

### Quality Assurance

Use QA to validate the changelogs and optionally apply them to a QA database. QA can use demo seed data if that helps test scenarios.

```bash
export FPT_DB_APP_PASSWORD_QA="<qa-password>"
scripts/db-env.sh qa validate
scripts/db-env.sh qa update
scripts/db-env.sh qa seed
scripts/db-env.sh qa verify
```

### Staging

Use staging to apply the same structural changes you intend to promote to production. Staging allows updates and verification, but it blocks demo seed data.

```bash
export FPT_DB_APP_PASSWORD_STAGE="<stage-password>"
scripts/db-env.sh stage validate
scripts/db-env.sh stage update
scripts/db-env.sh stage verify
scripts/db-env.sh stage status
```

### Production

Use production for validation, updates, verification, and status checks only. Production blocks bootstrap and demo seed operations.

```bash
export FPT_DB_APP_PASSWORD_PROD="<prod-password>"
scripts/db-env.sh prod validate
scripts/db-env.sh prod update
scripts/db-env.sh prod verify
scripts/db-env.sh prod status
```

Recommended promotion flow:

1. Run `qa validate`, then `qa update`.
2. Run `stage validate`, then `stage update`.
3. Run `prod validate`, then `prod update`.
4. Run `verify` and `status` after each update.
