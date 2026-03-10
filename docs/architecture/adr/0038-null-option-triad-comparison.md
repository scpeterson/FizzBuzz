# ADR 0038: Null Option Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Null-handling migration is a foundational FP transition topic for imperative developers.

## Decision

Maintain a dedicated triad for null/option handling:

1. `imperative-null-handling`
2. `csharp-null-patterns`
3. `langext-option-pipeline`

## Consequences

### Positive

- Clear before/after contrast from null checks to Option pipelines.

### Negative

- Partial overlap with dedicated Option monad triad.

## References

- `Scott.FizzBuzz.Core/Demos/NullOptionTriad/ImperativeNullHandlingDemo.cs`
- `Scott.FizzBuzz.Core/Demos/NullOptionTriad/CSharpNullPatternsDemo.cs`
- `Scott.FizzBuzz.Core/Demos/NullOptionTriad/LanguageExtOptionPipelineDemo.cs`
