# ADR 0034: Monad Basics Cat Demo

- Status: Accepted
- Date: 2026-03-10

## Context

A beginner-friendly Option/Either comparison demo is needed before advanced monad scenarios.

## Decision

Keep `monad-basics-cat` as a standalone beginner monad demo.

## Consequences

### Positive

- Provides a gentle transition from null checks to Option/Either pipelines.

### Negative

- Some overlap with Option/Either triad examples.

## References

- `Scott.FizzBuzz.Core/Demos/MonadFoundations/MonadBasicsCatDemo.cs`
- `Scott.FizzBuzz.Core/MonadBasics/Cat.cs`
