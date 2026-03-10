# ADR 0033: Monadic Functions Demo

- Status: Accepted
- Date: 2026-03-10

## Context

The project needs a focused demo showing how monadic composition improves pure/impure function handling.

## Decision

Keep `monadic-functions` as a standalone explanatory demo in monad foundations.

## Consequences

### Positive

- Makes abstraction benefits concrete for imperative developers.

### Negative

- May duplicate concepts shown in monad triads.

## References

- `Scott.FizzBuzz.Core/Demos/MonadFoundations/MonadicFunctionsDemo.cs`
- `Scott.FizzBuzz.Core/MonadicFunctions/PureFunctions.cs`
- `Scott.FizzBuzz.Core/MonadicFunctions/ImpureFunctions.cs`
