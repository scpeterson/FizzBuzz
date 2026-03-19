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

1. `scripts/db-bootstrap.sh`
2. `scripts/db-migrate.sh`
3. `scripts/db-seed.sh`
4. `scripts/db-verify.sh`

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
