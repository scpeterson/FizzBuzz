# ADR 0047: Composition Root Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Imperative teams often resolve dependencies ad hoc at call sites. The teaching path needs a side-by-side comparison of dependency wiring styles.

## Decision

Maintain a dedicated Composition Root triad:

1. `imperative-composition-root`
2. `csharp-composition-root`
3. `langext-composition-root`

Use shared environment contracts for consistent dependency lookup behavior.

## Consequences

### Positive

- Demonstrates explicit dependency flow from imperative to functional styles.
- Makes composition-root concerns concrete without adding infrastructure complexity.

### Negative

- Adds additional demo surface area to maintain.

## References

- `Scott.FizzBuzz.Core/Demos/Shared/IFunctionalDemoEnvironment.cs`
- `Scott.FizzBuzz.Core/Demos/Shared/InMemoryFunctionalDemoEnvironment.cs`
- `Scott.FizzBuzz.Core/Demos/CompositionRootTriad/CompositionRootRules.cs`
- `Scott.FizzBuzz.Core/Demos/CompositionRootTriad/ImperativeCompositionRootComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CompositionRootTriad/CSharpCompositionRootComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CompositionRootTriad/LanguageExtCompositionRootComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/CompositionRootTriad/CompositionRootTriadShould.cs`
