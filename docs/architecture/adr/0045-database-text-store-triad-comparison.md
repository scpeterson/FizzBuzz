# ADR 0045: Database Text-Store Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Data persistence side effects need a small, local-store teaching path before full RDBMS examples.

## Decision

Maintain a text-store persistence triad:

1. `imperative-db-text-store`
2. `csharp-db-text-store`
3. `langext-db-text-store-eff`

## Consequences

### Positive

- Introduces side-effect handling with minimal infrastructure dependency.

### Negative

- Duplicates some concerns later shown in PostgreSQL triad.

## References

- `Scott.FizzBuzz.Core/Demos/DatabaseTextStoreTriad/ImperativeTextStoreDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabaseTextStoreTriad/CSharpFunctionalTextStoreDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabaseTextStoreTriad/LanguageExtEffTextStoreDatabaseDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DatabaseTextStoreTriad/TextStoreRecordCodec.cs`
