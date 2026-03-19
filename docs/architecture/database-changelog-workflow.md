# Database Changelog Workflow

This repository now uses Liquibase to orchestrate PostgreSQL bootstrap, schema migration, and seed execution while preserving SQL files for the actual database changes.

## Structure

- `db/bootstrap/`: SQL used by the admin-side Liquibase bootstrap changelog (`B*.sql`)
- `db/migrations/`: application-schema Liquibase SQL migrations (`V*.sql`)
- `db/seeds/`: application-data Liquibase SQL seeds (`S*.sql`)
- `db/verify/`: read-only verification queries (`Q*.sql`)
- `db/liquibase/`: Liquibase changelog XML files that reference the SQL files above
- `scripts/`: orchestration scripts (`db-*.sh`)
- `output/db-changelog/`: execution logs
- `tools/liquibase/postgresql-42.7.9.jar`: recommended local location for the PostgreSQL JDBC driver used by Liquibase

## Liquibase Changelogs

- Admin DB (`postgres` by default): `db/liquibase/bootstrap-changelog.xml`
- App DB (`functional_programming_triads_demo` by default):
  - `db/liquibase/app-migrations-changelog.xml`
  - `db/liquibase/app-seed-changelog.xml`

Liquibase tracks execution in the standard `databasechangelog` and `databasechangeloglock` tables. The reset script also clears the old `admin_bootstrap_history` table if it exists from the pre-Liquibase workflow.

## Environment Variables

- `DB_HOST` (default `localhost`)
- `DB_PORT` (default `5432`)
- `DB_ADMIN_USER` (default `postgres`)
- `DB_ADMIN_PASSWORD` (default empty)
- `DB_ADMIN_DB` (default `postgres`)
- `DB_APP_USER` (default `functional_programming_triads_app`)
- `DB_APP_PASSWORD` (default `functional_programming_triads_app_pw`)
- `DB_NAME` (default `functional_programming_triads_demo`)
- `LIQUIBASE_CMD` (default `liquibase`)
- `LIQUIBASE_JDBC_CLASSPATH` (default `tools/liquibase/postgresql-42.7.9.jar`)

`psql` authentication still follows PostgreSQL conventions (`PGPASSWORD`, `.pgpass`, etc.).

The JDBC driver is a local prerequisite and is not committed to the repository. Install it with:

```bash
mkdir -p tools/liquibase
curl -L --fail --output tools/liquibase/postgresql-42.7.9.jar https://jdbc.postgresql.org/download/postgresql-42.7.9.jar
```

## Commands

From repository root:

```bash
scripts/db-init.sh
```

Individual phases:

```bash
scripts/db-bootstrap.sh
scripts/db-migrate.sh
scripts/db-seed.sh
scripts/db-verify.sh
scripts/db-status.sh
```

Reset local DB (destructive):

```bash
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

## Runtime Demo Connection

The Postgres-based demos read connection details from:

- `FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION`

Example:

```bash
export FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app_pw"
```
