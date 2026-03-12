# ADR 0002: Output Abstraction via `IOutput`/`IStyledOutput`

- Status: Accepted
- Date: 2026-03-03

## Context

Demos historically wrote directly to `Console`, which made testing and side-effect control harder.

As the number of demos increased, output behavior needed to be standardized and testable.

## Decision

Introduce output interfaces:

- `IOutput` for basic line output
- `IStyledOutput` for color/style scopes

Refactor `OutputUtilities` and demos to route output through these interfaces.

Use `ConsoleOutput` as the production implementation.

Centralize test doubles for output contracts in a shared test utility module (`NullOutputSink`, `RecordingOutputSink`, `RecordingStyledOutputSink`) so output assertions remain consistent across demo test suites.

Keep rule/helper functions pure where practical, and treat demo classes as the presentation boundary that renders success/failure summaries through `IOutput`.

This applies equally to demos that perform IO:

- async workflow demos compute results first and render them at the demo boundary
- file/database demos isolate persistence at explicit boundaries and use `IOutput` only for user-facing summaries

## Alternatives Considered

- Keep direct `Console` usage
  - Rejected due to poor testability and tighter coupling.
- Use logging framework as primary output abstraction
  - Rejected because demos are user-facing instructional output, not app telemetry.

## Consequences

### Positive

- Easier unit testing with recording/fake outputs.
- Clear side-effect boundary.
- Retains optional styling without forcing it on all outputs.
- Allows pure LanguageExt rule paths to remain side-effect free while still presenting comparable CLI output.
- Keeps infrastructure-oriented demos consistent with the rest of the codebase by rendering persistence/async outcomes through the same output boundary.

### Negative

- Slightly larger API surface.
- Requires constructor injection updates on demos.

## References

- `Scott.FizzBuzz.Core/Interfaces/IOutput.cs`
- `Scott.FizzBuzz.Core/Interfaces/IStyledOutput.cs`
- `Scott.FizzBuzz.Core/ConsoleOutput.cs`
- `Scott.FizzBuzz.Core/OutputUtilities.cs`
- `Scott.FizzBuzz.Core.Tests/TestUtilities/TestOutputSinks.cs`
