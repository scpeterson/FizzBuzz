using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

public class LanguageExtEffPostgresDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtEffPostgresDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtEffPostgresDatabaseDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-db-postgres-eff";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "database", "postgres", "io", "eff"];
    public string Description => "LanguageExt PostgreSQL workflow with pure transforms and DB IO at the Eff boundary.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt PostgreSQL Database Demo",
            BuildProgram(name, number)
                .Run()
                .Match(
                    Succ: result => result,
                    Fail: error => Left<string, PostgresPersonRecord>(error.Message)),
            (output, record) =>
            {
                output.WriteLine("Result: database upsert succeeded.");
                output.WriteLine($"Record: id={record.Id}, name={record.Name}, age={record.Age}");
            });

    internal static Eff<Either<string, PostgresPersonRecord>> BuildProgram(string? name, string? number) =>
        Eff(() =>
            PostgresDemoConfiguration.GetConnectionString()
                .Bind(connectionString =>
                    PostgresDatabaseInputParser.Parse(name, number)
                        .Bind(input => PostgresPersonStore
                            .EnsureSchema(connectionString)
                            .Bind(_ => PostgresPersonStore.UpsertByName(connectionString, input)))));
}
