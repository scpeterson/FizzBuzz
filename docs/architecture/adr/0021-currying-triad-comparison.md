# ADR 0021: Currying Triad Comparison

- Status: Accepted
- Date: 2026-03-09

## Context

The codebase compares imperative, C#/.NET functional, and LanguageExt approaches for core FP concepts. Currying was previously represented by a single demo, but not as a dedicated triad that shows the same business calculation across all three styles.

## Decision

Add a dedicated Currying triad under `Demos/CurryingTriad`:

1. `ImperativeCurryingComparisonDemo`
2. `CSharpCurryingComparisonDemo`
3. `LanguageExtCurryingComparisonDemo`

Use a shared rule module (`CurryingTriadRules`) so all variants compare identical inputs and pricing behavior.

## Alternatives Considered

- Keep only the existing single `CurryingDemo`
  - Rejected because it does not provide side-by-side triad parity with other FP topics.

## Consequences

### Positive

- Clear, repeatable comparison of non-curried vs curried flow.
- Demonstrates partial application in C# and functional composition in LanguageExt.
- Keeps behavior consistent via shared pure rules.

### Negative

- Additional demo and run-config surface area to maintain.

## References

- `Scott.FizzBuzz.Core/Demos/CurryingTriad/CurryingTriadRules.cs`
- `Scott.FizzBuzz.Core/Demos/CurryingTriad/ImperativeCurryingComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CurryingTriad/CSharpCurryingComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CurryingTriad/LanguageExtCurryingComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/CurryingTriad/CurryingTriadShould.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
