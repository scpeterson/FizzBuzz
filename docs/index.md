# Scott.FunctionalProgrammingTriads Documentation

This site hosts architecture documentation and decision records for the project.

## Start Here

Learning path for imperative programmers:

### Supporting C# Features

- [Pattern Matching Demo Baseline](architecture/adr/0025-pattern-matching-demo-baseline.md)
- [Tuple Demo Baseline](architecture/adr/0026-tuple-demo-baseline.md)

### Baseline Comparisons

- [Imperative Demo Baseline](architecture/adr/0022-imperative-demo-baseline.md)
- [Range Iteration Demo Baseline](architecture/adr/0023-range-iteration-demo-baseline.md)
- [Currying Baseline Demo](architecture/adr/0027-currying-baseline-demo.md)

### Core Triads

- [Null Option Triad Comparison](architecture/adr/0037-null-option-triad-comparison.md)
- [Parse Validate Triad Comparison](architecture/adr/0038-parse-validate-triad-comparison.md)
- [Validation Accumulation Triad Comparison](architecture/adr/0039-validation-accumulation-triad-comparison.md)
- [End-to-End Registration Triad Comparison](architecture/adr/0043-end-to-end-registration-triad-comparison.md)

### Effect / Infrastructure Triads

- [Async Eff Workflow Triad Comparison](architecture/adr/0042-async-eff-workflow-triad-comparison.md)
- [Database Text-Store Triad Comparison](architecture/adr/0044-database-text-store-triad-comparison.md)
- [Database PostgreSQL Triad Comparison](architecture/adr/0045-database-postgres-triad-comparison.md)

### LanguageExt Monads And Advanced Demos

- [Learning Path](architecture/learning-path.md)
- [Architecture Overview](architecture/README.md)
- [How To Compare A Triad](architecture/how-to-compare-a-triad.md)
- [Demo Author Checklist](architecture/demo-author-checklist.md)
- [Repo Health Notes](architecture/repo-health.md)
- [ADR 0001: Demo Triad Structure](architecture/adr/0001-demo-triad-structure.md)
- [ADR 0002: Output Abstraction via IOutput](architecture/adr/0002-output-abstraction-ioutput.md)
- [ADR 0003: Demo Discovery Metadata](architecture/adr/0003-demo-discovery-metadata.md)
- [ADR 0004: Private Logic Extraction](architecture/adr/0004-private-logic-extraction.md)
- [ADR 0005: Coverage Gates and Test Strategy](architecture/adr/0005-coverage-gates-and-test-strategy.md)
- [ADR 0006: Text-Store Persistence Side-Effect Boundaries](architecture/adr/0006-text-store-persistence-side-effect-boundaries.md)
- [ADR 0007: PostgreSQL Liquibase-Orchestrated SQL Workflow](architecture/adr/0007-postgresql-sql-first-changelog-workflow.md)
- [ADR 0008: LanguageExt Eff/Aff Effect Boundaries](architecture/adr/0008-languageext-eff-aff-effect-boundaries.md)
- [ADR 0009: Option Monad Triad Comparison](architecture/adr/0009-option-monad-triad-comparison.md)
- [ADR 0010: Either Monad Triad Comparison](architecture/adr/0010-either-monad-triad-comparison.md)
- [ADR 0011: Validation Monad Triad Comparison](architecture/adr/0011-validation-monad-triad-comparison.md)
- [ADR 0012: Reader Monad Triad Comparison](architecture/adr/0012-reader-monad-triad-comparison.md)
- [ADR 0013: State Monad Triad Comparison](architecture/adr/0013-state-monad-triad-comparison.md)
- [ADR 0014: IO Monad Triad Comparison](architecture/adr/0014-io-monad-triad-comparison.md)
- [ADR 0015: Try Monad Triad Comparison](architecture/adr/0015-try-monad-triad-comparison.md)
- [ADR 0016: Eff Monad Triad Comparison](architecture/adr/0016-eff-monad-triad-comparison.md)
- [ADR 0017: Aff Monad Triad Comparison](architecture/adr/0017-aff-monad-triad-comparison.md)
- [ADR 0018: Seq Monad Triad Comparison](architecture/adr/0018-seq-monad-triad-comparison.md)
- [ADR 0019: Writer Monad Triad Comparison](architecture/adr/0019-writer-monad-triad-comparison.md)
- [ADR 0020: TryOption Monad Triad Comparison](architecture/adr/0020-tryoption-monad-triad-comparison.md)
- [ADR 0021: Currying Triad Comparison](architecture/adr/0021-currying-triad-comparison.md)
- [ADR 0022: Imperative Demo Baseline](architecture/adr/0022-imperative-demo-baseline.md)
- [ADR 0023: Range Iteration Demo Baseline](architecture/adr/0023-range-iteration-demo-baseline.md)
- [ADR 0024: No Dependency Demo Baseline](architecture/adr/0024-no-dependency-demo-baseline.md)
- [ADR 0025: Pattern Matching Demo Baseline](architecture/adr/0025-pattern-matching-demo-baseline.md)
- [ADR 0026: Tuple Demo Baseline](architecture/adr/0026-tuple-demo-baseline.md)
- [ADR 0027: Currying Baseline Demo](architecture/adr/0027-currying-baseline-demo.md)
- [ADR 0028: Applicative Validation Demo](architecture/adr/0028-applicative-validation-demo.md)
- [ADR 0029: Either Baseline Demo](architecture/adr/0029-either-baseline-demo.md)
- [ADR 0030: LanguageExt Baseline Demo](architecture/adr/0030-languageext-baseline-demo.md)
- [ADR 0031: Schrodinger's Cat Demo](architecture/adr/0031-schrodingers-cat-demo.md)
- [ADR 0032: Monadic Functions Demo](architecture/adr/0032-monadic-functions-demo.md)
- [ADR 0033: Monad Basics Cat Demo](architecture/adr/0033-monad-basics-cat-demo.md)
- [ADR 0034: Other Monads Demo](architecture/adr/0034-other-monads-demo.md)
- [ADR 0035: .NET 10 Strict JSON Validation Demo](architecture/adr/0035-dotnet10-json-strict-validation-demo.md)
- [ADR 0036: .NET 10 Extension Members and Typeclass-style Demo](architecture/adr/0036-dotnet10-extension-members-typeclass-demo.md)
- [ADR 0037: Null Option Triad Comparison](architecture/adr/0037-null-option-triad-comparison.md)
- [ADR 0038: Parse Validate Triad Comparison](architecture/adr/0038-parse-validate-triad-comparison.md)
- [ADR 0039: Validation Accumulation Triad Comparison](architecture/adr/0039-validation-accumulation-triad-comparison.md)
- [ADR 0040: Collections Aggregation Triad Comparison](architecture/adr/0040-collections-aggregation-triad-comparison.md)
- [ADR 0041: Exception Boundaries Triad Comparison](architecture/adr/0041-exception-boundaries-triad-comparison.md)
- [ADR 0042: Async Eff Workflow Triad Comparison](architecture/adr/0042-async-eff-workflow-triad-comparison.md)
- [ADR 0043: End-to-End Registration Triad Comparison](architecture/adr/0043-end-to-end-registration-triad-comparison.md)
- [ADR 0044: Database Text-Store Triad Comparison](architecture/adr/0044-database-text-store-triad-comparison.md)
- [ADR 0045: Database PostgreSQL Triad Comparison](architecture/adr/0045-database-postgres-triad-comparison.md)
- [ADR 0046: Composition Root Triad Comparison](architecture/adr/0046-composition-root-triad-comparison.md)
- [ADR 0047: Domain Workflow Triad Comparison](architecture/adr/0047-domain-workflow-triad-comparison.md)
- [ADR 0048: Idempotent Command Triad Comparison](architecture/adr/0048-idempotent-command-triad-comparison.md)
- [ADR 0049: Retries + Backoff as Pure Policy Triad Comparison](architecture/adr/0049-retries-backoff-pure-policy-triad-comparison.md)
- [ADR 0050: Streaming / Large Data Processing Triad Comparison](architecture/adr/0050-streaming-large-data-processing-triad-comparison.md)
- [ADR 0051: Concurrency Safety Triad Comparison](architecture/adr/0051-concurrency-safety-triad-comparison.md)
- [ADR 0052: Event Sourcing Lite Triad Comparison](architecture/adr/0052-event-sourcing-lite-triad-comparison.md)
- [ADR 0053: Configuration Validation at Startup Triad Comparison](architecture/adr/0053-configuration-validation-startup-triad-comparison.md)
- [ADR 0054: Demo Execution Result Contract](architecture/adr/0054-demo-execution-result-contract.md)
- [PostgreSQL Changelog Workflow](architecture/database-changelog-workflow.md)
- [PostgreSQL Operations Runbook](architecture/database-operations-runbook.md)
- [Architecture Docs Contributing Guide](architecture/contributing.md)
- [Documentation Tags](architecture/tags.md)
- [Versioned Docs with Mike](architecture/versioning.md)
- [PDF Export](architecture/pdf-export.md)
- [System Overview Diagram](architecture/diagrams/system-overview.md)

## Local Preview

From the repository root:

```bash
rm -rf .venv-docs
/opt/homebrew/bin/python3.10 -m venv .venv-docs
source .venv-docs/bin/activate
pip install -r requirements-docs.txt
mkdocs serve
```

Then open [http://127.0.0.1:8000](http://127.0.0.1:8000).

### Python Path Notes

Use the Python 3.10 executable that matches your Homebrew installation:

- Apple Silicon (M1/M2/M3): `/opt/homebrew/bin/python3.10`
- Intel macOS: `/usr/local/bin/python3.10`

If `mkdocs` is not found after activation, run:

```bash
python -m mkdocs serve
```
