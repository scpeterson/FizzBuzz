# ADR 0007: PostgreSQL SQL-First Changelog Workflow

- Status: Accepted
- Date: 2026-03-05

## Context

The project now uses PostgreSQL for database demos. We need a repeatable way to:

- create DB prerequisites (role/database/grants),
- apply schema changes,
- seed test/demo data,
- and preserve a changelog of executed SQL.

## Decision

Adopt a SQL-first workflow using versioned SQL files and shell orchestration scripts.

- Bootstrap SQL is stored in `db/bootstrap/B*.sql` and tracked in `public.admin_bootstrap_history` (admin DB).
- App migrations are stored in `db/migrations/V*.sql` and tracked in `public.db_migration_history` (app DB).
- Default bootstrap/runtime naming follows the repository name (`functional_programming_triads_demo`, `functional_programming_triads_app`, `FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION`).
- Seed SQL is stored in `db/seeds/S*.sql` and tracked in `public.db_migration_history` with `kind='seed'`.
- Verification SQL is stored in `db/verify/Q*.sql` and executed as read-only checks.
- All phases emit timestamped execution logs under `output/db-changelog/`.

## Alternatives Considered

- Use ad hoc manual SQL execution
  - Rejected due to weak traceability and repeatability.
- Introduce a migration framework first (Flyway/Liquibase)
  - Deferred. We keep SQL-first scripts now to minimize tooling dependencies and can migrate later.
- Keep DB demo-only code-level table creation
  - Rejected because it hides DDL history and bypasses explicit changelog requirements.

## Consequences

### Positive

- Full SQL lifecycle is versioned in-repo.
- Deterministic local rebuild (`db-reset.sh`).
- Auditable execution history and log artifacts.
- Database role, database name, and connection environment variable now align with the repository identity instead of the earlier FizzBuzz naming.

### Negative

- Shell scripts become part of maintenance surface area.
- Requires disciplined naming and ordering of SQL files.
- Bootstrap history lives outside the app DB by design.

## References

- `db/bootstrap/`
- `db/migrations/`
- `db/seeds/`
- `db/verify/`
- `scripts/db-init.sh`
- `scripts/db-bootstrap.sh`
- `scripts/db-migrate.sh`
- `scripts/db-seed.sh`
- `scripts/db-verify.sh`
- `scripts/db-status.sh`
- `scripts/db-reset.sh`
- `docs/architecture/database-changelog-workflow.md`
