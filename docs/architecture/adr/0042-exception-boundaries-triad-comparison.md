# ADR 0042: Exception Boundaries Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Exception boundaries are critical for converting exception-driven code into value-oriented flows.

## Decision

Maintain an exception-boundaries triad:

1. `imperative-exception-boundaries`
2. `csharp-result-recovery`
3. `langext-try-either-recovery`

## Consequences

### Positive

- Demonstrates boundary isolation and explicit error channels.

### Negative

- Overlaps with Try-based demos by design.

## References

- `Scott.FizzBuzz.Core/Demos/ExceptionBoundariesTriad/ImperativeExceptionBoundariesDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ExceptionBoundariesTriad/CSharpResultRecoveryDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ExceptionBoundariesTriad/LanguageExtTryEitherRecoveryDemo.cs`
