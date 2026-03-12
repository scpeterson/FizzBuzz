# ADR 0055: Demo Execution Result Contract

- Status: Accepted
- Date: 2026-03-12

## Context

The demo triads are intended to compare:

1. Imperative style
2. Functional style in plain C#/.NET
3. Functional style with LanguageExt

Even after moving shared rule kernels toward plain .NET, every demo still implemented `IDemo.Run` as `Either<string, Unit>`. That left a LanguageExt-shaped boundary in every imperative and plain C# demo, even when the internals were no longer using LanguageExt.

## Decision

Use an application-level result contract for demo execution:

- `DemoExecutionResult.Success()`
- `DemoExecutionResult.Failure(string error)`

`IDemo.Run` and `DemoRunner.Execute` now return `DemoExecutionResult` instead of `Either<string, Unit>`.

LanguageExt remains available inside LanguageExt-specific rules and demos, but it is no longer required by the top-level demo contract.

Plain C# demos may still use local result records internally where helpful, but those types stay inside the demo/topic and do not leak into the shared demo execution surface.

## Alternatives Considered

- Keep `Either<string, Unit>` as the `IDemo` contract
  - Rejected because it makes imperative and plain C# demos present a LanguageExt API at the application boundary.
- Replace all internal functional code with custom result types
  - Rejected because LanguageExt is still the point of comparison for the functional third member of each triad.

## Consequences

### Positive

- Imperative and plain C# demos no longer expose a LanguageExt-shaped execution contract.
- The console runner and tests can reason about success/failure using a small app-specific type.
- LanguageExt use is clearer because it now appears where it is intentionally being taught, not in the shared execution surface.
- Plain C# demos can model topic-local pipelines with small local result types without reintroducing LanguageExt at the application boundary.

### Negative

- Test helpers needed a parallel assertion path for `DemoExecutionResult`.
- Some utility methods and entrypoints required mechanical migration away from `Either<string, Unit>`.

## References

- `Scott.FizzBuzz.Core/DemoExecutionResult.cs`
- `Scott.FizzBuzz.Core/Interfaces/IDemo.cs`
- `Scott.FizzBuzz.Console/DemoRunner.cs`
- `Scott.FizzBuzz.Console/Program.cs`
