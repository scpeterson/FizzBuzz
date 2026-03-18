# ADR 0045: Database PostgreSQL Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

The project includes real database workflow comparisons and needs a dedicated triad ADR for runtime behavior and effect boundaries.

## Decision

Maintain a PostgreSQL persistence triad:

1. `imperative-db-postgres`
2. `csharp-db-postgres`
3. `langext-db-postgres-eff`

The triad reads its runtime connection from `FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION`. A temporary fallback to the legacy `FIZZBUZZ_POSTGRES_CONNECTION` variable is retained for local compatibility during the rename transition.

## Consequences

### Positive

- Demonstrates production-style side-effect handling with a real database.
- Uses database-facing names that now match the renamed repository and solution.

### Negative

- Requires environmental setup and migration discipline.
- The temporary legacy environment-variable fallback should eventually be removed once local setups have migrated.

## References

- `Scott.FunctionalProgrammingTriads.Core/Demos/DatabasePostgresTriad/ImperativePostgresDatabaseDemo.cs`
- `Scott.FunctionalProgrammingTriads.Core/Demos/DatabasePostgresTriad/CSharpFunctionalPostgresDatabaseDemo.cs`
- `Scott.FunctionalProgrammingTriads.Core/Demos/DatabasePostgresTriad/LanguageExtEffPostgresDatabaseDemo.cs`
- `Scott.FunctionalProgrammingTriads.Core/Demos/DatabasePostgresTriad/PostgresPersonStore.cs`
- `docs/architecture/adr/0007-postgresql-sql-first-changelog-workflow.md`
