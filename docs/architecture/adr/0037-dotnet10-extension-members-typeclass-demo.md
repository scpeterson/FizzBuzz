# ADR 0037: .NET 10 Extension Members and Typeclass-style Demo

- Status: Accepted
- Date: 2026-03-10

## Context

The codebase demonstrates modern extension-member style techniques for functional composition in current C#/.NET.

## Decision

Keep `fp-extension-members-typeclasses` as a dedicated feature demo.

## Consequences

### Positive

- Shows concise typeclass-style extension composition patterns.

### Negative

- Feature semantics can evolve across language/toolchain versions.

## References

- `Scott.FizzBuzz.Core/Demos/DotNet10Features/FpExtensionMembersTypeclassesDemo.cs`
- `Scott.FizzBuzz.Core/Demos/DotNet10Features/EitherTypeclassExtensions.cs`
