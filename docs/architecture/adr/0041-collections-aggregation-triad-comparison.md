# ADR 0041: Collections Aggregation Triad Comparison

- Status: Accepted
- Date: 2026-03-10

## Context

Collection aggregation is a frequent imperative-to-functional migration point.

## Decision

Maintain a collections/aggregation triad:

1. `imperative-collections-aggregation`
2. `csharp-collections-aggregation`
3. `langext-seq-aggregation`

## Consequences

### Positive

- Shows practical sequence processing improvements.

### Negative

- Some conceptual overlap with Seq monad triad.

## References

- `Scott.FizzBuzz.Core/Demos/CollectionsAggregationTriad/ImperativeCollectionsAggregationDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CollectionsAggregationTriad/CSharpCollectionsAggregationDemo.cs`
- `Scott.FizzBuzz.Core/Demos/CollectionsAggregationTriad/LanguageExtSeqAggregationDemo.cs`
