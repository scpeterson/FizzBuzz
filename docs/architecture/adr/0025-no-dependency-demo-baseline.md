# ADR 0025: No Dependency Demo Baseline

- Status: Accepted
- Date: 2026-03-10

## Context

The teaching sequence includes a no-library baseline to show functional ideas in plain C#.

## Decision

Keep `no-dependency` as a dedicated baseline demo without LanguageExt dependencies.

## Consequences

### Positive

- Demonstrates that FP techniques can start with core language constructs.

### Negative

- Requires separate maintenance from LanguageExt variants.

## References

- `Scott.FizzBuzz.Core/Demos/Baseline/NoDependencyDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
