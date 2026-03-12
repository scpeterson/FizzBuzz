# Learning Path

This repository is easiest to use as a staged comparison tool rather than a flat catalog of demos.

## Recommended Order

1. Supporting C# features
   - `pattern-matching`
   - `tuple-demo`

2. Baseline comparisons
   - `imperative`
   - `range-iter`
   - `demo-currying`
   - `demo-either`
   - `lang-ext`

3. Core comparison triads
   - `imperative-null-handling`
   - `csharp-null-patterns`
   - `langext-option-pipeline`
   - `imperative-parse-validate`
   - `csharp-parse-validate-pipeline`
   - `langext-either-parse-validate`
   - `imperative-validation-first-error`
   - `csharp-validation-error-list`
   - `langext-validation-accumulate`
   - `imperative-user-registration`
   - `csharp-functional-registration`
   - `langext-functional-registration`

4. Effect and infrastructure triads
   - `imperative-async-workflow`
   - `csharp-async-composition`
   - `langext-eff-async-workflow`
   - `imperative-db-text-store`
   - `csharp-db-text-store`
   - `langext-db-text-store-eff`
   - `imperative-db-postgres`
   - `csharp-db-postgres`
   - `langext-db-postgres-eff`

5. LanguageExt monad comparisons
   - `csharp-option-comparison` / `langext-option-monad-comparison`
   - `csharp-either-comparison` / `langext-either-monad-comparison`
   - `csharp-validation-monad-comparison` / `langext-validation-monad-comparison`
   - `csharp-reader-comparison` / `langext-reader-monad-comparison`
   - `csharp-state-comparison` / `langext-state-monad-comparison`
   - `csharp-io-comparison` / `langext-io-monad-comparison`
   - `csharp-try-monad-comparison` / `langext-try-monad-comparison`
   - `csharp-tryoption-monad-comparison` / `langext-tryoption-monad-comparison`
   - `csharp-seq-monad-comparison` / `langext-seq-monad-comparison`
   - `csharp-writer-monad-comparison` / `langext-writer-monad-comparison`
   - `csharp-eff-monad-comparison` / `langext-eff-monad-comparison`
   - `csharp-aff-monad-comparison` / `langext-aff-monad-comparison`

6. Advanced and .NET 10 material
   - `fp-extension-members-typeclasses`
   - `fp-json-strict-validation`
   - `other-monads`

## How To Read A Triad

For each topic:

1. Run the imperative demo first to see the familiar baseline.
2. Run the plain C# demo next to see how far core language features can take you.
3. Run the LanguageExt demo last to see what the extra abstraction buys you.

## CLI Tip

Use the demo list to browse in the same general order:

```bash
dotnet run --project Scott.FizzBuzz.Console -- --list
```

The list is intentionally ordered to surface supporting features first, then baseline demos, then comparison triads, and finally more advanced material.
