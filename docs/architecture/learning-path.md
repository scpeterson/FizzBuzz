# Learning Path

This repository is easiest to use as a staged comparison tool rather than a flat catalog of demos.

## First Hour Path

If you want the quickest useful introduction, start here:

1. [`pattern-matching`](adr/0025-pattern-matching-demo-baseline.md)
2. [`tuple-demo`](adr/0026-tuple-demo-baseline.md)
3. [`imperative`](adr/0022-imperative-demo-baseline.md)
4. [`demo-currying`](adr/0027-currying-baseline-demo.md)
5. [`csharp-parse-validate-pipeline`](adr/0038-parse-validate-triad-comparison.md)
6. [`csharp-null-patterns`](adr/0037-null-option-triad-comparison.md)
7. [`csharp-validation-error-list`](adr/0039-validation-accumulation-triad-comparison.md)
8. [`langext-option-monad-comparison`](adr/0009-option-monad-triad-comparison.md)

This sequence gives you a gentle ramp from helpful C# language tools to plain C# functional style, then one representative LanguageExt monad comparison.

## Recommended Order

### Supporting C# features

- [`pattern-matching`](adr/0025-pattern-matching-demo-baseline.md)
- [`tuple-demo`](adr/0026-tuple-demo-baseline.md)

### Baseline comparisons

- [`imperative`](adr/0022-imperative-demo-baseline.md)
- [`range-iter`](adr/0023-range-iteration-demo-baseline.md)
- [`demo-currying`](adr/0027-currying-baseline-demo.md)
- [`demo-either`](adr/0029-either-baseline-demo.md)
- [`lang-ext`](adr/0030-languageext-baseline-demo.md)

### Core comparison triads

- [`imperative-null-handling`](adr/0037-null-option-triad-comparison.md)
- [`csharp-null-patterns`](adr/0037-null-option-triad-comparison.md)
- [`langext-option-pipeline`](adr/0037-null-option-triad-comparison.md)
- [`imperative-parse-validate`](adr/0038-parse-validate-triad-comparison.md)
- [`csharp-parse-validate-pipeline`](adr/0038-parse-validate-triad-comparison.md)
- [`langext-either-parse-validate`](adr/0038-parse-validate-triad-comparison.md)
- [`imperative-validation-first-error`](adr/0039-validation-accumulation-triad-comparison.md)
- [`csharp-validation-error-list`](adr/0039-validation-accumulation-triad-comparison.md)
- [`langext-validation-accumulate`](adr/0039-validation-accumulation-triad-comparison.md)
- [`imperative-user-registration`](adr/0043-end-to-end-registration-triad-comparison.md)
- [`csharp-functional-registration`](adr/0043-end-to-end-registration-triad-comparison.md)
- [`langext-functional-registration`](adr/0043-end-to-end-registration-triad-comparison.md)

### Effect and infrastructure triads

- [`imperative-async-workflow`](adr/0042-async-eff-workflow-triad-comparison.md)
- [`csharp-async-composition`](adr/0042-async-eff-workflow-triad-comparison.md)
- [`langext-eff-async-workflow`](adr/0042-async-eff-workflow-triad-comparison.md)
- [`imperative-db-text-store`](adr/0044-database-text-store-triad-comparison.md)
- [`csharp-db-text-store`](adr/0044-database-text-store-triad-comparison.md)
- [`langext-db-text-store-eff`](adr/0044-database-text-store-triad-comparison.md)
- [`imperative-db-postgres`](adr/0045-database-postgres-triad-comparison.md)
- [`csharp-db-postgres`](adr/0045-database-postgres-triad-comparison.md)
- [`langext-db-postgres-eff`](adr/0045-database-postgres-triad-comparison.md)

Postgres demos require the database setup workflow from the database runbook. For local development, run `scripts/db-init.sh` or `scripts/db-env.sh dev init` first.

### LanguageExt monad comparisons

- [`csharp-option-comparison`](adr/0009-option-monad-triad-comparison.md) / [`langext-option-monad-comparison`](adr/0009-option-monad-triad-comparison.md)
- [`csharp-either-comparison`](adr/0010-either-monad-triad-comparison.md) / [`langext-either-monad-comparison`](adr/0010-either-monad-triad-comparison.md)
- [`csharp-validation-monad-comparison`](adr/0011-validation-monad-triad-comparison.md) / [`langext-validation-monad-comparison`](adr/0011-validation-monad-triad-comparison.md)
- [`csharp-reader-comparison`](adr/0012-reader-monad-triad-comparison.md) / [`langext-reader-monad-comparison`](adr/0012-reader-monad-triad-comparison.md)
- [`csharp-state-comparison`](adr/0013-state-monad-triad-comparison.md) / [`langext-state-monad-comparison`](adr/0013-state-monad-triad-comparison.md)
- [`csharp-io-comparison`](adr/0014-io-monad-triad-comparison.md) / [`langext-io-monad-comparison`](adr/0014-io-monad-triad-comparison.md)
- [`csharp-try-monad-comparison`](adr/0015-try-monad-triad-comparison.md) / [`langext-try-monad-comparison`](adr/0015-try-monad-triad-comparison.md)
- [`csharp-tryoption-monad-comparison`](adr/0020-tryoption-monad-triad-comparison.md) / [`langext-tryoption-monad-comparison`](adr/0020-tryoption-monad-triad-comparison.md)
- [`csharp-seq-monad-comparison`](adr/0018-seq-monad-triad-comparison.md) / [`langext-seq-monad-comparison`](adr/0018-seq-monad-triad-comparison.md)
- [`csharp-writer-monad-comparison`](adr/0019-writer-monad-triad-comparison.md) / [`langext-writer-monad-comparison`](adr/0019-writer-monad-triad-comparison.md)
- [`csharp-eff-monad-comparison`](adr/0016-eff-monad-triad-comparison.md) / [`langext-eff-monad-comparison`](adr/0016-eff-monad-triad-comparison.md)
- [`csharp-aff-monad-comparison`](adr/0017-aff-monad-triad-comparison.md) / [`langext-aff-monad-comparison`](adr/0017-aff-monad-triad-comparison.md)

### Advanced and .NET 10 material

- [`fp-extension-members-typeclasses`](adr/0036-dotnet10-extension-members-typeclass-demo.md)
- [`fp-json-strict-validation`](adr/0035-dotnet10-json-strict-validation-demo.md)
- [`other-monads`](adr/0034-other-monads-demo.md)

## How To Read A Triad

For each topic:

1. Run the imperative demo first to see the familiar baseline.
2. Run the plain C# demo next to see how far core language features can take you.
3. Run the LanguageExt demo last to see what the extra abstraction buys you.

## CLI Tip

Use the demo list to browse in the same general order:

```bash
dotnet run \
  --project Scott.FunctionalProgrammingTriads.Console/Scott.FunctionalProgrammingTriads.Console.csproj \
  -- --list
```

The list is intentionally ordered to surface supporting features first, then baseline demos, then comparison triads, and finally more advanced material.

## Related Guides

- `docs/architecture/how-to-compare-a-triad.md`
- `docs/architecture/demo-author-checklist.md`
