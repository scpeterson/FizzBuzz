using LanguageExt;
using Scott.FunctionalProgrammingTriads.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FunctionalProgrammingTriads.Core.Demos.DatabaseTextStoreTriad;

public class LanguageExtEffTextStoreDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtEffTextStoreDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtEffTextStoreDatabaseDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-db-text-store-eff";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "database", "io", "text-store", "eff"];
    public string Description => "LanguageExt pipeline with pure transforms and file IO at the Eff boundary.";

    public DemoExecutionResult Run(string? name, string? number)
    {
        var filePath = BuildTempPath();

        try
        {
            var result = SaveAndReadBack(filePath, name, number)
                .Run()
                .Match(
                    Succ: success => success,
                    Fail: error => Left<string, PersonRecord>(error.Message));

            return FunctionalDemoOutput.Render(
                _output,
                "LanguageExt Text-Store Database Demo",
                result,
                (output, record) =>
                {
                    output.WriteLine("Result: database save succeeded.");
                    output.WriteLine($"Record: id={record.Id}, name={record.Name}, age={record.Age}");
                });
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    internal static Eff<Either<string, PersonRecord>> SaveAndReadBack(string filePath, string? name, string? number) =>
        Eff(() =>
            ParseInput(name, number)
                .Bind(input =>
                    LoadRecords(filePath)
                        .Map(records => TextStoreRecordCodec.Upsert(records, input))
                        .Bind(updated => SaveRecords(filePath, updated)
                            .Bind(_ => TextStoreRecordCodec
                                .FindById(updated, input.Id)
                                .ToEither($"Record '{input.Id}' was not found after save.")))));

    private static Either<string, PersonRecord> ParseInput(string? name, string? number)
    {
        var sanitizedName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();

        if (!int.TryParse(number ?? "21", out var age))
            return Left<string, PersonRecord>("Age must be an integer.");

        if (age < 0)
            return Left<string, PersonRecord>("Age must be non-negative.");

        return Right<string, PersonRecord>(new PersonRecord(1, sanitizedName, age));
    }

    private static Either<string, Seq<PersonRecord>> LoadRecords(string filePath)
    {
        var content = File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : string.Empty;

        return TextStoreRecordCodec.Parse(content);
    }

    private static Either<string, Unit> SaveRecords(string filePath, Seq<PersonRecord> records)
    {
        File.WriteAllText(filePath, TextStoreRecordCodec.Serialize(records));
        return unit;
    }

    private static string BuildTempPath() =>
        Path.Combine(Path.GetTempPath(), $"functional-programming-triads-db-langext-{Guid.NewGuid():N}.txt");
}
