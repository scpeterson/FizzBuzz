# ADR 0007: PostgreSQL Liquibase-Orchestrated SQL Workflow

- Status: Accepted
- Date: 2026-03-05

## Context

The project now uses PostgreSQL for database demos. We need a repeatable way to:

- create DB prerequisites (role/database/grants),
- apply schema changes,
- seed test/demo data,
- and preserve a changelog of executed SQL.

## Decision

Adopt a Liquibase-driven workflow that keeps the database changes primarily in SQL files and uses Liquibase changelogs as the orchestration layer.

- Bootstrap SQL is stored in `db/bootstrap/B*.sql` and executed through `db/liquibase/bootstrap-changelog.xml` against the admin DB.
- App migrations are stored in `db/migrations/V*.sql` and executed through `db/liquibase/app-migrations-changelog.xml`.
- Reference data is stored under `db/reference-data/` and executed through `db/liquibase/app-reference-data-changelog.xml`.
- Demo seed SQL is stored in `db/seeds/S*.sql` and executed through `db/liquibase/app-demo-seed-changelog.xml`.
- Verification SQL is stored in `db/verify/Q*.sql` and executed as read-only checks outside Liquibase.
- Liquibase tracks execution through `databasechangelog` and `databasechangeloglock`.
- Environment-specific defaults are supported for `dev`, `qa`, `stage`, and `prod`, each with separate Liquibase property files and database names/users.
- Default local runtime naming follows the repository name (`functional_programming_triads_demo`, `functional_programming_triads_app`, `FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION`).
- All phases emit timestamped execution logs under `output/db-changelog/`.

## Alternatives Considered

- Use ad hoc manual SQL execution
  - Rejected due to weak traceability and repeatability.
- Keep the previous hand-rolled SQL history tables and shell loops
  - Rejected because Liquibase now gives us repeatable ordering, checksum tracking, and a standard changelog model without taking the SQL itself away from the repository.
- Keep DB demo-only code-level table creation
  - Rejected because it hides DDL history and bypasses explicit changelog requirements.

## Consequences

### Positive

- Full SQL lifecycle is versioned in-repo.
- Deterministic local rebuild (`db-reset.sh`).
- Auditable execution history and log artifacts.
- Database role, database name, and connection environment variable now align with the repository identity instead of the earlier FizzBuzz naming.
- The project now uses a standard Liquibase workflow while still keeping the actual DDL and seed work in SQL files.
- Local reset now clears both Liquibase tracking tables and the old custom bootstrap history if present.

### Negative

- Shell scripts remain part of the maintenance surface area, though bootstrap/migrate/seed now share a single `db-update.sh` implementation.
- Requires disciplined naming and ordering of SQL files.
- Liquibase adds a JDBC dependency and changelog XML maintenance surface.
- Local setup now depends on a PostgreSQL JDBC jar being available for Liquibase, typically under `tools/liquibase/` or via `LIQUIBASE_JDBC_CLASSPATH`.

## References

- `db/bootstrap/`
- `db/migrations/`
- `db/reference-data/`
- `db/seeds/`
- `db/verify/`
- `scripts/db-init.sh`
- `config/liquibase/`
- `scripts/db-env.sh`
- `scripts/db-update.sh`
- `scripts/db-bootstrap.sh`
- `scripts/db-migrate.sh`
- `scripts/db-seed.sh`
- `scripts/db-verify.sh`
- `scripts/db-status.sh`
- `scripts/db-reset.sh`
- `docs/architecture/database-changelog-workflow.md`
