# ADR 0008: LanguageExt Eff/Aff Effect Boundaries

- Status: Accepted
- Date: 2026-03-08

## Context

The project teaches imperative developers by comparing imperative, C# functional, and LanguageExt variants of the same workflow.

As async and side-effecting demos expanded, LanguageExt examples needed a consistent model for:

1. synchronous effects (`Eff`)
2. asynchronous effects (`Aff`)
3. pure domain transforms (`Either`/`Option`/pure functions)

Without an explicit decision, demos risk mixing pure logic with IO or using inconsistent runtime patterns.

## Decision

For LanguageExt-based demos that perform side effects:

- Model synchronous side effects with `Eff<T>`.
- Model asynchronous side effects with `Aff<T>`.
- Keep validation and transformations pure outside effect boundaries.
- At synchronous demo entry points (`IDemo.Run`), bridge `Aff` execution via `Run().AsTask().GetAwaiter().GetResult()` to avoid `ValueTask` misuse (CA2012).

Triad topics may still include imperative and core C# functional variants; the LanguageExt variant is the only one required to use `Eff`/`Aff`.

## Alternatives Considered

- Use `Task` directly in LanguageExt demos and skip `Aff`
  - Rejected because it weakens demonstration of LanguageExt effect abstractions.
- Force `Eff`/`Aff` into all C# functional demos
  - Rejected because the C# functional track intentionally avoids LanguageExt-heavy constructs.
- Keep side-effecting logic inline in LanguageExt demos
  - Rejected because it blurs pure-vs-effect boundaries and reduces teaching clarity.

## Consequences

### Positive

- Clear and repeatable effect model for LanguageExt demos.
- Better teaching contrast between imperative side effects and functional effect boundaries.
- Cleaner analyzer posture around `ValueTask` consumption.

### Negative

- Adds abstraction overhead for newcomers.
- Requires discipline to preserve purity in helper functions.

## References

- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/LanguageExtAsyncEffWorkflowDemo.cs`
- `Scott.FizzBuzz.Core/Demos/MonadFoundations/OtherMonadsDemo.cs`
- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/ImperativeAsyncWorkflowDemo.cs`
- `Scott.FizzBuzz.Core/Demos/AsyncEffTriad/CSharpAsyncCompositionDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/NegativeDemoPathsShould.cs`
