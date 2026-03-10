# ADR 0043: Async Eff Workflow Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Async workflows with side effects are a high-value teaching area for effect-boundary design.

## Decision

Maintain an async workflow triad:

1. `imperative-async-workflow`
2. `csharp-async-composition`
3. `langext-eff-async-workflow`

## Consequences

### Positive

- Clarifies async composition and effect boundary differences.

### Negative

- Requires ongoing alignment with async/effect APIs.

## References

- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/ImperativeAsyncWorkflowDemo.cs`
- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/CSharpAsyncCompositionDemo.cs`
- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/LanguageExtAsyncEffWorkflowDemo.cs`
