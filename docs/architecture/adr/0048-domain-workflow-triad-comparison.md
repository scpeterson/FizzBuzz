# ADR 0048: Domain Workflow Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Imperative workflow code often encodes state with booleans/strings. The teaching path needs explicit state-transition modeling comparisons.

## Decision

Maintain a dedicated Domain Workflow triad:

1. `imperative-domain-workflow`
2. `csharp-domain-workflow`
3. `langext-domain-workflow`

Use explicit domain states and transition functions to compare imperative versus functional orchestration.

## Consequences

### Positive

- Improves clarity of state transitions and invalid-state prevention.
- Demonstrates functional domain modeling in a practical workflow.

### Negative

- Introduces more domain-specific types to maintain.

## References

- `Scott.FizzBuzz.Core/Demos/DomainWorkflowTriad/DomainWorkflowRules.cs`
- `Scott.FizzBuzz.Core/Demos/DomainWorkflowTriad/ImperativeDomainWorkflowComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DomainWorkflowTriad/CSharpDomainWorkflowComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DomainWorkflowTriad/LanguageExtDomainWorkflowComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/DomainWorkflowTriad/DomainWorkflowTriadShould.cs`
