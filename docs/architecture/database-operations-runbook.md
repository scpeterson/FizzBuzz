# PostgreSQL Database Operations Runbook

Use this runbook for day-to-day execution. Keep ADRs for architectural decisions and use this document for operational steps.

## When To Use Which Document

- `architecture/adr/*.md`: why decisions were made
- `architecture/database-changelog-workflow.md`: structure/rules
- `architecture/database-operations-runbook.md` (this file): exactly what to run and when

## Prerequisites

1. PostgreSQL is installed and running.
2. `psql` is available on your PATH.
3. You are in the repository root:

```bash
cd /Users/scottpeterson/Dev/FizzBuzz/Scott.FizzBuzz
```

1. If needed, set admin/app credentials:

```bash
export DB_HOST=localhost
export DB_PORT=5432
export DB_ADMIN_USER=postgres
export DB_ADMIN_DB=postgres
export DB_APP_USER=functional_programming_triads_app
export DB_APP_PASSWORD=functional_programming_triads_app_pw
export DB_NAME=functional_programming_triads_demo
```

## First-Time Setup (From Scratch)

Run all phases in order (bootstrap, migrate, seed, verify):

```bash
scripts/db-init.sh
```

What this does:

1. Creates/updates role and database (`db/bootstrap/*`).
2. Applies versioned schema SQL (`db/migrations/*`).
3. Loads seed data (`db/seeds/*`).
4. Runs verify queries (`db/verify/*`).
5. Writes logs to `output/db-changelog/`.

## Configure Demo Runtime Connection

Set this before running PostgreSQL demos:

```bash
export FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app_pw"
```

## Normal Development Workflow

### 1. Check status/history

```bash
scripts/db-status.sh
```

### 2. Add schema changes

1. Add a new `V*.sql` in `db/migrations/`.
2. Apply migrations:

```bash
scripts/db-migrate.sh
```

### 3. Add/update seed data

1. Add or update `S*.sql` in `db/seeds/`.
2. Apply seeds:

```bash
scripts/db-seed.sh
```

### 4. Validate DB state

```bash
scripts/db-verify.sh
```

## Cleaning Data Without Dropping the Database

Use this when you want to keep schema and migration history but refresh demo rows.

Option A: targeted delete (recommended for iterative work)

```bash
psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_APP_USER" -d "$DB_NAME" -c "DELETE FROM demo_people WHERE name IN ('Scott', 'Guest');"
```

Option B: reset all rows and identity in the demo table

```bash
psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_APP_USER" -d "$DB_NAME" -c "TRUNCATE TABLE demo_people RESTART IDENTITY;"
```

Then reapply seed data if needed:

```bash
scripts/db-seed.sh
```

## Full Reset (Drop and Rebuild Database)

Use when changing bootstrap assumptions or when local state is inconsistent.

```bash
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

This is destructive for the target database.

## If You Change Existing SQL Files

Rules:

1. Do not edit an applied `V*.sql` migration. Create a new migration instead.
2. If you changed a `B*.sql` or `S*.sql` file intentionally, rerun its phase.
3. If local history is no longer trustworthy, run a full reset.

## Common Command Sequences

### Rebuild everything from zero

```bash
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

### Apply only new schema migration(s)

```bash
scripts/db-migrate.sh
scripts/db-verify.sh
```

### Clear test/demo rows and reseed

```bash
psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_APP_USER" -d "$DB_NAME" -c "TRUNCATE TABLE demo_people RESTART IDENTITY;"
scripts/db-seed.sh
scripts/db-verify.sh
```

## Rider Run Configurations

Available run configs include:

- `DB - init`
- `DB - status`
- `DB - reset (safe)`
- `DB - reset (confirmed)`

And demo configs:

- `Demos - imperative-db-postgres`
- `Demos - csharp-db-postgres`
- `Demos - langext-db-postgres-eff`
- `Triad - Database Postgres - 01 Imperative`
- `Triad - Database Postgres - 02 CSharp`
- `Triad - Database Postgres - 03 LanguageExt`
