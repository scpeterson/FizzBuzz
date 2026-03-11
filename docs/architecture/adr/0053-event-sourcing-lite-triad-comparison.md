# ADR 0053: Event Sourcing Lite Triad Comparison

- Status: Accepted
- Date: 2026-03-11

## Context

Event sourcing is a useful bridge concept for imperative developers moving toward functional techniques, because it separates:

1. Facts that happened (events)
2. Current state projection (replay/fold)
3. New intent handling (append new events)

The codebase needed a minimal, deterministic demo that shows these mechanics without introducing external infrastructure.

## Decision

Add a dedicated `EventSourcingLiteTriad` with three demos:

1. `imperative-event-sourcing-lite-comparison`
2. `csharp-event-sourcing-lite-comparison`
3. `langext-event-sourcing-lite-comparison`

Add shared rules in `EventSourcingLiteRules` to provide:

- Input validation for stream id and deposit amount
- Seeded in-memory event history
- State projection via replay (`AccountProjection`)
- Command handling by appending events to history
- Equivalent execution flows for imperative, C#, and LanguageExt variants

The LanguageExt variant remains pure and returns `Either<string, Unit>` without output side effects.

## Consequences

### Positive

- Demonstrates event sourcing fundamentals using the existing triad teaching model.
- Makes replay-based state derivation explicit and testable.
- Keeps the LanguageExt path purely functional while preserving comparable outcomes.

### Negative

- This is a "lite" event model and does not include persistence, snapshots, or concurrency control.
- Adds more demo, test, and run-configuration surface area to maintain.

## References

- `Scott.FizzBuzz.Core/Demos/EventSourcingLiteTriad/EventSourcingLiteRules.cs`
- `Scott.FizzBuzz.Core/Demos/EventSourcingLiteTriad/ImperativeEventSourcingLiteComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/EventSourcingLiteTriad/CSharpEventSourcingLiteComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/EventSourcingLiteTriad/LanguageExtEventSourcingLiteComparisonDemo.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/EventSourcingLiteTriad/EventSourcingLiteTriadShould.cs`
- `.run/Triad - Event Sourcing Lite - 01 Imperative.run.xml`
- `.run/Triad - Event Sourcing Lite - 02 CSharp.run.xml`
- `.run/Triad - Event Sourcing Lite - 03 LanguageExt.run.xml`
