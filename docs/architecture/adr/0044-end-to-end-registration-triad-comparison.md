# ADR 0044: End-to-End Registration Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

A full mini-feature scenario is needed to compare orchestration patterns end to end.

## Decision

Maintain an end-to-end registration triad:

1. `imperative-user-registration`
2. `csharp-functional-registration`
3. `langext-functional-registration`

## Consequences

### Positive

- Demonstrates how FP scales from local functions to feature-level workflows.

### Negative

- Higher maintenance due to broader scenario scope.

## References

- `Scott.FizzBuzz.Core/Demos/EndToEndMiniFeatureTriad/ImperativeUserRegistrationDemo.cs`
- `Scott.FizzBuzz.Core/Demos/EndToEndMiniFeatureTriad/CSharpFunctionalRegistrationDemo.cs`
- `Scott.FizzBuzz.Core/Demos/EndToEndMiniFeatureTriad/LanguageExtFunctionalRegistrationDemo.cs`
