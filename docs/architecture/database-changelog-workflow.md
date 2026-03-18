# PostgreSQL Changelog Workflow

This project uses SQL-first changelogs for database lifecycle management in local development.

For day-to-day commands and cleanup/reset procedures, see:

- `architecture/database-operations-runbook.md`

## Goals

- Keep all SQL changes versioned and reviewable.
- Preserve an auditable execution history for bootstrap and app-level SQL.
- Support deterministic reset/rebuild for demos and tests.

## Directory Layout

- `db/bootstrap/`: cluster-level SQL (role/database/grants)
- `db/migrations/`: schema/version changes (`V*.sql`)
- `db/seeds/`: test/demo data (`S*.sql`)
- `db/verify/`: read-only verification queries (`Q*.sql`)
- `scripts/`: orchestration scripts (`db-*.sh`)
- `output/db-changelog/`: execution logs

## History Tables

- Admin DB (`postgres`): `public.admin_bootstrap_history`
- App DB (`functional_programming_triads_demo` by default): `public.db_migration_history`

Both store:

- script name
- checksum (SHA-256)
- execution timestamp
- executing user

## Environment Variables

- `DB_HOST` (default `localhost`)
- `DB_PORT` (default `5432`)
- `DB_ADMIN_USER` (default `postgres`)
- `DB_ADMIN_DB` (default `postgres`)
- `DB_APP_USER` (default `functional_programming_triads_app`)
- `DB_APP_PASSWORD` (default `functional_programming_triads_app_pw`)
- `DB_NAME` (default `functional_programming_triads_demo`)

`psql` authentication still follows PostgreSQL conventions (`PGPASSWORD`, `.pgpass`, etc.).

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

## Change Rules

1. Never edit an already-applied `V*.sql` file.
2. Add a new sequential migration for every schema change.
3. Keep one concern per migration.
4. Treat seed scripts as idempotent where practical (`ON CONFLICT`, `MERGE`-style updates).
5. Keep verify scripts read-only.

## Runtime Demo Connection

The Postgres-based demos read connection details from:

- `FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION`

Example:

```bash
export FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app_pw"
```
