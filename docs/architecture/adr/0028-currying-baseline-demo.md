# ADR 0028: Currying Baseline Demo

- Status: Accepted
- Date: 2026-03-10

## Context

A baseline currying demo complements the full currying triad by introducing the concept in isolation.

## Decision

Keep `demo-currying` as a standalone baseline while maintaining `CurryingTriad` for full comparison.

## Consequences

### Positive

- Supports incremental learning before triad-level comparisons.

### Negative

- Conceptual overlap with ADR 0021.

## References

- `Scott.FizzBuzz.Core/Demos/CurryingBaseline/CurryingDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
- `docs/architecture/adr/0021-currying-triad-comparison.md`
