# ADR 0054: Configuration Validation at Startup Triad Comparison

- Status: Accepted
- Date: 2026-03-11

## Context

Startup configuration is a common source of production failures. Imperative systems often defer or scatter config checks, which increases runtime risk and weakens operator feedback.

To help imperative developers adopt functional techniques, the codebase needs a direct comparison of startup validation styles:

1. Imperative fail-fast branching
2. C# composed validation with aggregated checks
3. LanguageExt applicative validation with accumulated errors

## Decision

Add a dedicated `ConfigurationValidationStartupTriad` with three demos:

1. `imperative-startup-config-validation-comparison`
2. `csharp-startup-config-validation-comparison`
3. `langext-startup-config-validation-comparison`

Add shared rules in `ConfigurationValidationStartupRules` to provide:

- Startup config candidate construction from profile and timeout input
- Consistent rule set for environment, connection string, timeout, retries, and prod-only constraints
- Imperative fail-fast validation path
- C# aggregated validation path
- LanguageExt `Validation<Seq<string>, StartupConfig>` applicative path with error accumulation

The LanguageExt variant remains pure and side-effect free, returning `Either<string, Unit>`.

## Consequences

### Positive

- Demonstrates a practical startup hardening pattern in triad format.
- Shows clear contrast between fail-fast and accumulated-validation strategies.
- Keeps configuration policy deterministic and directly unit-testable.

### Negative

- Adds another triad surface area (demos/tests/configs/docs) to maintain.
- Uses synthetic profile-based config candidates rather than external config providers.

## References

- `Scott.FizzBuzz.Core/Demos/ConfigurationValidationStartupTriad/ConfigurationValidationStartupRules.cs`
- `Scott.FizzBuzz.Core/Demos/ConfigurationValidationStartupTriad/ImperativeConfigurationValidationStartupComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ConfigurationValidationStartupTriad/CSharpConfigurationValidationStartupComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/ConfigurationValidationStartupTriad/LanguageExtConfigurationValidationStartupComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/ConfigurationValidationStartupTriad/ConfigurationValidationStartupTriadShould.cs`
- `.run/Triad - Startup Config Validation - 01 Imperative.run.xml`
- `.run/Triad - Startup Config Validation - 02 CSharp.run.xml`
- `.run/Triad - Startup Config Validation - 03 LanguageExt.run.xml`
