# Scott.FizzBuzz

A teaching codebase for imperative developers learning functional programming in C#.

The project demonstrates common tasks in a triad format:

1. Imperative C#
2. Functional C# (core language/.NET)
3. Functional C# using LanguageExt

## Solution Structure

- `Scott.FizzBuzz.Console`: CLI host and demo runner
- `Scott.FizzBuzz.Core`: demo implementations and shared logic
- `Scott.FizzBuzz.Core.Tests`: core tests
- `Scott.FizzBuzz.Console.Tests`: console/registration tests
- `docs/`: architecture docs and ADRs (MkDocs site)
- `db/`: SQL changelog files (bootstrap, migrations, seeds, verify)
- `scripts/`: database automation scripts

## Prerequisites

- .NET 10 SDK
- PostgreSQL (for DB demos)
- Python 3.10 (for docs site)

## Run Demos

From repo root:

```bash
dotnet run --project Scott.FizzBuzz.Console -- --list
dotnet run --project Scott.FizzBuzz.Console -- -m imperative
dotnet run --project Scott.FizzBuzz.Console -- -m langext-db-postgres-eff -n Scott -b 21
```

For PostgreSQL demos, set:

```bash
export FIZZBUZZ_POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=fizzbuzz_demo;Username=fizzbuzz_app;Password=fizzbuzz_app_pw"
```

## Database Setup (PostgreSQL)

Initialize role/database/schema/seeds/verification:

```bash
scripts/db-init.sh
```

Useful commands:

```bash
scripts/db-status.sh
scripts/db-migrate.sh
scripts/db-seed.sh
scripts/db-verify.sh
DB_RESET_CONFIRM=YES scripts/db-reset.sh
```

Detailed operations guide:

- `docs/architecture/database-operations-runbook.md`

## Build and Test

```bash
dotnet build
dotnet test Scott.FizzBuzz.Core.Tests/Scott.FizzBuzz.Core.Tests.csproj
dotnet test Scott.FizzBuzz.Console.Tests/Scott.FizzBuzz.Console.Tests.csproj
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
- `docs/architecture/adr/`
