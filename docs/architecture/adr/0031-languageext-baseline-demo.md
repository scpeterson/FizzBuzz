# ADR 0031: LanguageExt Baseline Demo

- Status: Accepted
- Date: 2026-03-10

## Context

A baseline LanguageExt demo provides a low-friction entry point before topic-specific triads.

## Decision

Keep `lang-ext` as a standalone LanguageExt baseline demo.

## Consequences

### Positive

- Lowers onboarding cost for LanguageExt syntax and conventions.

### Negative

- Overlaps with specific monad triads in scope.

## References

- `Scott.FizzBuzz.Core/Demos/BaselineLanguageExt/LanguageExtDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
