# Scott.FunctionalProgrammingTriads

A teaching codebase for imperative developers learning functional programming in C#. The project began with FizzBuzz comparisons and has expanded into a broader triad-based functional programming guide.

The project demonstrates common tasks in a triad format:

1. Imperative C#
2. Functional C# (core language/.NET)
3. Functional C# using LanguageExt

## First Hour Path

If you want the shortest path through the repository, start with:

1. `pattern-matching`
2. `tuple-demo`
3. `imperative`
4. `demo-currying`
5. `csharp-parse-validate-pipeline`
6. `csharp-null-patterns`
7. `csharp-validation-error-list`
8. `langext-option-monad-comparison`

Then move into the broader learning path below.

If you're approaching the codebase from an imperative background, the smoothest order is:

1. Supporting C# language features
   - `pattern-matching`
   - `tuple-demo`
2. Baseline comparisons
   - `imperative`
   - `range-iter`
   - `demo-currying`
3. Core triads
   - `csharp-parse-validate-pipeline`
   - `csharp-null-patterns`
   - `csharp-validation-error-list`
   - `csharp-functional-registration`
4. Effect and infrastructure triads
   - `csharp-async-composition`
   - `csharp-db-text-store`
   - `csharp-db-postgres`
5. LanguageExt monad triads and advanced demos
   - `langext-option-monad-comparison`
   - `langext-either-monad-comparison`
   - `langext-validation-monad-comparison`
   - `langext-eff-async-workflow`

`dotnet run --project Scott.FunctionalProgrammingTriads.Console/Scott.FunctionalProgrammingTriads.Console.csproj -- --list` now follows this learning path more closely.

## Solution Structure

- `Scott.FunctionalProgrammingTriads.Console`: CLI host and demo runner
- `Scott.FunctionalProgrammingTriads.Core`: demo implementations and shared logic
- `Scott.FunctionalProgrammingTriads.Core.Tests`: core tests
- `Scott.FunctionalProgrammingTriads.Console.Tests`: console/registration tests
- `docs/`: architecture docs and ADRs (MkDocs site)
- `db/`: SQL and Liquibase changelog files for bootstrap, migrations, seeds, and verify
- `scripts/`: database automation scripts

## Prerequisites

- .NET 10 SDK
- PostgreSQL (for DB demos)
- Liquibase 5.0.2
- Python 3.10 (for docs site)

## Run Demos

From repo root:

```bash
dotnet run --project Scott.FunctionalProgrammingTriads.Console/Scott.FunctionalProgrammingTriads.Console.csproj -- --list
dotnet run --project Scott.FunctionalProgrammingTriads.Console/Scott.FunctionalProgrammingTriads.Console.csproj -- -m imperative
dotnet run --project Scott.FunctionalProgrammingTriads.Console/Scott.FunctionalProgrammingTriads.Console.csproj -- -m langext-db-postgres-eff -n Scott -b 21
```

For PostgreSQL demos, set:

```bash
export FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app_pw"
```

## Database Setup (PostgreSQL)

Install the PostgreSQL JDBC driver for local Liquibase runs:

```bash
mkdir -p tools/liquibase
curl -L --fail --output tools/liquibase/postgresql-42.7.9.jar https://jdbc.postgresql.org/download/postgresql-42.7.9.jar
```

If you keep the driver somewhere else, set `LIQUIBASE_JDBC_CLASSPATH` to that jar path before running the DB scripts.

Initialize role/database/schema/seeds/verification with Liquibase orchestration:

```bash
scripts/db-init.sh
```

Useful commands:

```bash
scripts/db-status.sh
scripts/db-update.sh bootstrap
scripts/db-update.sh migrate
scripts/db-update.sh reference
scripts/db-update.sh seed
scripts/db-verify.sh
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

Environment-aware workflow examples:

```bash
scripts/db-env.sh dev init
scripts/db-env.sh qa validate
scripts/db-env.sh stage update
scripts/db-env.sh prod status
```

Detailed operations guide:

- `docs/architecture/database-operations-runbook.md`

## Build and Test

```bash
dotnet build Scott.FunctionalProgrammingTriads.sln
dotnet test Scott.FunctionalProgrammingTriads.Core.Tests/Scott.FunctionalProgrammingTriads.Core.Tests.csproj
dotnet test Scott.FunctionalProgrammingTriads.Console.Tests/Scott.FunctionalProgrammingTriads.Console.Tests.csproj
```

## Documentation Site

```bash
/opt/homebrew/bin/python3.10 -m venv .venv-docs
source .venv-docs/bin/activate
pip install -r requirements-docs.txt
mkdocs serve
```

Open: [http://127.0.0.1:8000](http://127.0.0.1:8000)

## Architecture and ADRs

- `docs/architecture/README.md`
- `docs/architecture/learning-path.md`
- `docs/architecture/how-to-compare-a-triad.md`
- `docs/architecture/demo-author-checklist.md`
- `docs/architecture/repo-health.md`
- `docs/architecture/adr/`
