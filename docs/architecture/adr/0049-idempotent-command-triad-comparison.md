# ADR 0049: Idempotent Command Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Idempotency is a common backend requirement and a good example of side-effect control and state transition boundaries.

## Decision

Maintain a dedicated Idempotent Command triad:

1. `imperative-idempotent-command`
2. `csharp-idempotent-command`
3. `langext-idempotent-command`

Use a shared seeded set of processed command IDs in the environment for comparable behavior.

## Consequences

### Positive

- Demonstrates duplicate detection and command handling in three styles.
- Reinforces pure decision logic with explicit state updates.

### Negative

- Requires careful consistency between environment seed and demo expectations.

## References

- `Scott.FizzBuzz.Core/Demos/Shared/IFunctionalDemoEnvironment.cs`
- `Scott.FizzBuzz.Core/Demos/Shared/InMemoryFunctionalDemoEnvironment.cs`
- `Scott.FizzBuzz.Core/Demos/IdempotentCommandTriad/IdempotentCommandRules.cs`
- `Scott.FizzBuzz.Core/Demos/IdempotentCommandTriad/ImperativeIdempotentCommandComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/IdempotentCommandTriad/CSharpIdempotentCommandComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/IdempotentCommandTriad/LanguageExtIdempotentCommandComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/IdempotentCommandTriad/IdempotentCommandTriadShould.cs`
