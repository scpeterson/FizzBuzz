# Scott.FizzBuzz Documentation

This site hosts architecture documentation and decision records for the project.

## Start Here

- [Architecture Overview](architecture/README.md)
- [ADR 0001: Demo Triad Structure](architecture/adr/0001-demo-triad-structure.md)
- [ADR 0002: Output Abstraction via IOutput](architecture/adr/0002-output-abstraction-ioutput.md)
- [ADR 0003: Demo Discovery Metadata](architecture/adr/0003-demo-discovery-metadata.md)
- [ADR 0004: Private Logic Extraction](architecture/adr/0004-private-logic-extraction.md)
- [ADR 0005: Coverage Gates and Test Strategy](architecture/adr/0005-coverage-gates-and-test-strategy.md)
- [ADR 0006: Text-Store Persistence Side-Effect Boundaries](architecture/adr/0006-text-store-persistence-side-effect-boundaries.md)
- [ADR 0007: PostgreSQL SQL-First Changelog Workflow](architecture/adr/0007-postgresql-sql-first-changelog-workflow.md)
- [ADR 0008: LanguageExt Eff/Aff Effect Boundaries](architecture/adr/0008-languageext-eff-aff-effect-boundaries.md)
- [ADR 0009: Option Monad Triad Comparison](architecture/adr/0009-option-monad-triad-comparison.md)
- [ADR 0010: Monad Triad Expansion](architecture/adr/0010-monad-triad-expansion.md)
- [ADR 0011: Monad Triad Wave Two (Try/Eff/Aff/Seq/Writer)](architecture/adr/0011-monad-triad-wave-two-try-eff-aff-seq-writer.md)
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
