# ADR 0024: Range Iteration Demo Baseline

- Status: Accepted
- Date: 2026-03-10

## Context

Range-based iteration is used to contrast imperative loops with functional iteration concepts.

## Decision

Keep `range-iter` as a baseline demo showing range/iteration usage without full monadic abstraction.

## Consequences

### Positive

- Bridges imperative loops to functional-style sequence thinking.

### Negative

- Partial conceptual overlap with Seq-focused triads.

## References

- `Scott.FizzBuzz.Core/Demos/Baseline/RangeIterationDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
