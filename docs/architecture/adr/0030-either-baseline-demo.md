# ADR 0030: Either Baseline Demo

- Status: Accepted
- Date: 2026-03-10

## Context

An introductory Either demo helps imperative developers see error-as-data before the full Either triad.

## Decision

Keep `demo-either` as a standalone baseline demo.

## Consequences

### Positive

- Introduces core Either semantics with minimal setup.

### Negative

- Conceptual overlap with ADR 0010.

## References

- `Scott.FizzBuzz.Core/Demos/BaselineLanguageExt/EitherDemo.cs`
- `Scott.FizzBuzz.Core/Demos/BaselineLanguageExt/EitherValidation.cs`
- `docs/architecture/adr/0010-either-monad-triad-comparison.md`
