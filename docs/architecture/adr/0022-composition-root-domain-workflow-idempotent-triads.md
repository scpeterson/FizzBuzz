# ADR 0022: Composition Root, Domain Workflow, and Idempotent Command Triads

- Status: Superseded by ADR 0047, ADR 0048, ADR 0049
- Date: 2026-03-10

## Context

This ADR originally grouped three separate triad decisions into one record:

1. Composition root and dependency wiring
2. Domain workflow modeling with explicit state transitions
3. Idempotent command handling

## Decision

Supersede this grouped ADR with one ADR per demo concern:

1. ADR 0047: Composition Root Triad Comparison
2. ADR 0048: Domain Workflow Triad Comparison
3. ADR 0049: Idempotent Command Triad Comparison

## Alternatives Considered

Keep all three concerns in one ADR:
- Rejected to align with the project's "one ADR per demo" documentation rule.

## Consequences

### Positive

- ADR collection is consistent and easier to navigate by demo concern.
- Each triad decision can evolve independently.

### Negative

- Slightly more ADR files to maintain.

## References

- `docs/architecture/adr/0047-composition-root-triad-comparison.md`
- `docs/architecture/adr/0048-domain-workflow-triad-comparison.md`
- `docs/architecture/adr/0049-idempotent-command-triad-comparison.md`
