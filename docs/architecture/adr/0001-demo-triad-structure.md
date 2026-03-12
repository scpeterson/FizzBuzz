# ADR 0001: Demo Triad Structure

- Status: Accepted
- Date: 2026-03-03

## Context

The project goal is to help imperative programmers understand functional programming by comparing equivalent approaches.

Without a consistent structure, comparisons become ad hoc and difficult to teach.

## Decision

Organize demo topics as triads where practical:

1. Imperative
2. C# functional style
3. LanguageExt functional style

Use consistent metadata and runnable keys so each variant can be listed and executed independently.

Keep console presentation comparable across triad members, even when the LanguageExt variant internally computes through pure functions and only renders output at the outer demo boundary.

Keep shared kernels plain .NET where practical:

- Shared sample data, records, constants, and deterministic calculations may be reused across all three variants.
- Shared rule files used by imperative and C# variants should avoid LanguageExt types such as `Either`, `Option`, `Validation`, `Seq`, `Reader`, `State`, `Eff`, and `Aff`.
- Shared environment seams used by imperative and C# variants should also prefer plain `Try...` methods, nullable values, or app-local result types over LanguageExt return types.
- LanguageExt-specific lifting and monadic composition should live in LanguageExt-specific adapter/rule files so the teaching contrast remains honest.

Apply the same separation rule to side-effect-heavy topics as well:

- Async workflows should use plain `Task`, `await`, exceptions, or app-local result types in imperative/C# variants.
- File and database demos should keep parsing, transformation, and result modeling plain .NET in imperative/C# variants, while reserving `Eff`, `Aff`, `Either`, and other LanguageExt abstractions for the LanguageExt member of the triad.

## Alternatives Considered

- Single "best" implementation per topic
  - Rejected because it removes side-by-side learning value.
- Separate repositories per paradigm
  - Rejected due to operational overhead and weaker discoverability.

## Consequences

### Positive

- Clear comparative learning path.
- Easier to add and review new topics.
- Better run-configuration support for side-by-side execution.
- Makes behavioral comparison easier because each variant reports success and failure in a similar user-facing shape.
- Keeps imperative and plain C# examples pedagogically clean by preventing shared LanguageExt abstractions from leaking into those variants.
- Preserves the intended contrast even in infrastructure-oriented demos such as async workflows, text-store persistence, and PostgreSQL persistence.

### Negative

- More files/classes to maintain.
- Requires discipline to keep parity across triad variants.
- Some duplication is accepted in exchange for clearer teaching boundaries.

## References

- `Scott.FizzBuzz.Core/Demos`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
- `.run/Triad - *.run.xml`
