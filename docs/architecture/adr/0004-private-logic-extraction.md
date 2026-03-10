# ADR 0004: Extract Private Demo Logic into Pure Helpers

- Status: Accepted
- Date: 2026-03-03

## Context

Several demos contained significant private parsing/validation logic.

Private method behavior was only indirectly covered through demo execution tests, which limited precision for branch testing and made regressions harder to diagnose.

## Decision

Extract private logic into dedicated pure helper classes, then test helpers directly.

Keep demo classes focused on orchestration and output.

## Alternatives Considered

- Test private methods via reflection
  - Rejected because it is brittle and discourages clean boundaries.
- Leave logic private and rely on integration tests only
  - Rejected due to lower branch confidence and weaker failure isolation.

## Consequences

### Positive

- Direct branch-level tests for core logic.
- Cleaner demo classes and clearer responsibilities.
- Easier reuse across related demos.

### Negative

- More files/types to manage.
- Requires naming discipline for helper classes.

## References

- `Scott.FizzBuzz.Core/Demos/MonadFoundations/OtherMonadsValidation.cs`
- `Scott.FizzBuzz.Core/Demos/BaselineLanguageExt/EitherValidation.cs`
- `Scott.FizzBuzz.Core/Demos/DotNet10Features/FpJsonStrictValidationLogic.cs`
- `Scott.FizzBuzz.Core/Demos/EndToEndMiniFeatureTriad/CSharpFunctionalRegistrationLogic.cs`
- `Scott.FizzBuzz.Core/Demos/EndToEndMiniFeatureTriad/LanguageExtFunctionalRegistrationLogic.cs`
