# ADR 0040: Validation Accumulation Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Imperative validation often fails fast; functional styles can accumulate errors.

## Decision

Maintain a dedicated validation-accumulation triad:

1. `imperative-validation-first-error`
2. `csharp-validation-error-list`
3. `langext-validation-accumulate`

## Consequences

### Positive

- Explicitly demonstrates fail-fast vs accumulation tradeoffs.

### Negative

- Some overlap with validation monad triad.

## References

- `Scott.FizzBuzz.Core/Demos/ValidationAccumulationTriad/ImperativeValidationFirstErrorDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ValidationAccumulationTriad/CSharpValidationErrorListDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ValidationAccumulationTriad/LanguageExtValidationAccumulateDemo.cs`
