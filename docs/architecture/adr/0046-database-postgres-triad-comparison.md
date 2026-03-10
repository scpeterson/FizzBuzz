# ADR 0046: Database PostgreSQL Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

The project includes real database workflow comparisons and needs a dedicated triad ADR for runtime behavior and effect boundaries.

## Decision

Maintain a PostgreSQL persistence triad:

1. `imperative-db-postgres`
2. `csharp-db-postgres`
3. `langext-db-postgres-eff`

## Consequences

### Positive

- Demonstrates production-style side-effect handling with a real database.

### Negative

- Requires environmental setup and migration discipline.

## References

- `Scott.FizzBuzz.Core/Demos/DatabasePostgresTriad/ImperativePostgresDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabasePostgresTriad/CSharpFunctionalPostgresDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabasePostgresTriad/LanguageExtEffPostgresDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabasePostgresTriad/PostgresPersonStore.cs`
- `docs/architecture/adr/0007-postgresql-sql-first-changelog-workflow.md`
