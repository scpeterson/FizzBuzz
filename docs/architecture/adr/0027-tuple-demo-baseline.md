# ADR 0027: Tuple Demo Baseline

- Status: Accepted
- Date: 2026-03-10

## Context

Tuples are used as a lightweight functional data-shaping tool in core C#.

## Decision

Keep `tuple-demo` as a standalone baseline demo introducing tuple-centric transformation patterns.

## Consequences

### Positive

- Shows simple immutable grouping without custom types.

### Negative

- Less expressive than dedicated domain records in advanced examples.

## References

- `Scott.FizzBuzz.Core/Demos/Baseline/TupleDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
