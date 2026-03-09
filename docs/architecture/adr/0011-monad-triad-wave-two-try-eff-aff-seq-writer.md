# ADR 0011: Monad Triad Wave Two (Try, Eff, Aff, Seq, Writer)

- Status: Accepted
- Date: 2026-03-09

## Context

The first monad expansion established dedicated triads for Option, Either, Validation, Reader, State, and IO-style effects.
To complete the practical monad coverage used in this codebase, additional dedicated triads were needed for:

- `Try`
- `Eff`
- `Aff`
- `Seq`
- `Writer`

Without explicit triads for these, key LanguageExt monadic patterns remained scattered across mixed demos and were harder to compare against imperative/C# baselines.

## Decision

Add five new triads, each with:

1. imperative comparison demo
2. C# functional comparison demo
3. LanguageExt monad demo

Each triad uses a shared scenario and helper rules file to keep behavior parity and emphasize orchestration differences.

The LanguageExt variants are side-effect minimal/pure in orchestration, with effect execution isolated to monad boundaries.

## Alternatives Considered

- Keep these monads only in aggregate demos like `OtherMonadsDemo`
  - Rejected because discoverability and side-by-side teaching value are weaker.
- Add only LanguageExt demos for these monads
  - Rejected because the teaching goal requires imperative and C# baselines.
- Reuse existing non-triad demos only (`ExceptionBoundariesTriad`, `AsyncEffTriad`, `CollectionsAggregationTriad`)
  - Rejected because those topics do not present monad-focused comparison as directly.

## Consequences

### Positive

- Consistent monad-by-monad learning path.
- Better CLI/run-config discoverability for specific monad topics.
- Improved test coverage for monad-specific behavior and negative paths.

### Negative

- Additional classes, tests, and run configs increase maintenance load.
- Some overlap with older demos requires clear naming and tags.

## References

- `Scott.FizzBuzz.Core/Demos/TryMonadTriad/LanguageExtTryMonadComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/EffMonadTriad/LanguageExtEffMonadComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/AffMonadTriad/LanguageExtAffMonadComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/SeqMonadTriad/LanguageExtSeqMonadComparisonDemo.cs`
- `Scott.FizzBuzz.Core/Demos/WriterMonadTriad/LanguageExtWriterMonadComparisonDemo.cs`
- `Scott.FizzBuzz.Console/DemoServiceRegistration.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/TryMonadTriad/TryMonadTriadShould.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/EffMonadTriad/EffMonadTriadShould.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/AffMonadTriad/AffMonadTriadShould.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/SeqMonadTriad/SeqMonadTriadShould.cs`
- `Scott.FizzBuzz.Core.Tests/Demos/WriterMonadTriad/WriterMonadTriadShould.cs`
