using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

public class LanguageExtEffPostgresDatabaseDemo : IDemo
{
    public const string DemoKey = "langext-db-postgres-eff";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "database", "postgres", "io", "eff"];
    public string Description => "LanguageExt PostgreSQL workflow with pure transforms and DB IO at the Eff boundary.";

    public Either<string, Unit> Run(string? name, string? number) =>
        BuildProgram(name, number)
            .Run()
            .Match(
                Succ: result => result.Map(_ => unit),
                Fail: error => Left<string, Unit>(error.Message));

    internal static Eff<Either<string, PostgresPersonRecord>> BuildProgram(string? name, string? number) =>
        Eff(() =>
            PostgresDemoConfiguration.GetConnectionString()
                .Bind(connectionString =>
                    PostgresDatabaseInputParser.Parse(name, number)
                        .Bind(input => PostgresPersonStore
                            .EnsureSchema(connectionString)
                            .Bind(_ => PostgresPersonStore.UpsertByName(connectionString, input)))));
}
