# ADR 0039: Parse Validate Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Parse/validate pipelines are common imperative workflows with strong FP payoff.

## Decision

Maintain a dedicated parse/validate triad:

1. `imperative-parse-validate`
2. `csharp-parse-validate-pipeline`
3. `langext-either-parse-validate`

## Consequences

### Positive

- Demonstrates progressive reduction of branching/orchestration noise.

### Negative

- Overlap with validation-focused demos is intentional but broader.

## References

- `Scott.FizzBuzz.Core/Demos/ParseValidateTriad/ImperativeParseValidateDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ParseValidateTriad/CSharpParseValidatePipelineDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ParseValidateTriad/LanguageExtEitherParseValidateDemo.cs`
