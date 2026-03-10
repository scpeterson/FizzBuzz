# ADR 0023: Imperative Demo Baseline

- Status: Accepted
- Date: 2026-03-10

## Context

The teaching path needs a clear imperative baseline for comparison with functional approaches.

## Decision

Keep a dedicated imperative baseline demo (`imperative`) that demonstrates traditional control flow and branching before introducing functional alternatives.

## Consequences

### Positive

- Establishes an explicit starting point for imperative developers.

### Negative

- Adds overlap with other baseline demos.

## References

- `Scott.FizzBuzz.Core/Demos/Baseline/ImperativeDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
