using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

public class CSharpFunctionalPostgresDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpFunctionalPostgresDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpFunctionalPostgresDatabaseDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-db-postgres";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "database", "postgres", "io"];
    public string Description => "C# functional PostgreSQL pipeline with pure input parsing and explicit IO boundaries.";

    public Either<string, Unit> Run(string? name, string? number) =>
        PostgresDemoConfiguration.GetConnectionString()
            .Bind(connectionString =>
                PostgresDatabaseInputParser.Parse(name, number)
                    .Bind(input => PostgresPersonStore
                        .EnsureSchema(connectionString)
                        .Bind(_ => PostgresPersonStore.UpsertByName(connectionString, input))))
            .Bind(saved =>
                ExecuteWithSpacing(
                    _output,
                    () => _output.WriteLine($"Saved row: {saved.Id}|{saved.Name}|{saved.Age}"),
                    "C# Functional PostgreSQL Database"));
}
