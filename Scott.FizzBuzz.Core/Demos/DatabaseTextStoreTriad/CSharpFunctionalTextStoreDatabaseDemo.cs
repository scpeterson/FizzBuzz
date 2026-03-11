using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DatabaseTextStoreTriad;

public class CSharpFunctionalTextStoreDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpFunctionalTextStoreDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpFunctionalTextStoreDatabaseDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-db-text-store";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "database", "io", "text-store"];
    public string Description => "C# functional core with file IO isolated to explicit boundaries.";

    public Either<string, Unit> Run(string? name, string? number)
    {
        var filePath = BuildTempPath();

        try
        {
            return SaveAndReadBack(filePath, name, number)
                .Bind(record =>
                    ExecuteWithSpacing(
                        _output,
                        () => _output.WriteLine($"Saved row: {TextStoreRecordCodec.SerializeLine(record)}"),
                        "C# Functional Text-Store Database"));
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    internal static Either<string, PersonRecord> SaveAndReadBack(string filePath, string? name, string? number) =>
        ParseInput(name, number)
            .Bind(input =>
                LoadRecords(filePath)
                    .Bind(records =>
                    {
                        var record = new PersonRecord(input.Id, input.Name, input.Age);
                        var updated = TextStoreRecordCodec.Upsert(records, record);
                        return SaveRecords(filePath, updated).Map(_ => record);
                    }));

    private static Either<string, (int Id, string Name, int Age)> ParseInput(string? name, string? number)
    {
        var sanitizedName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();

        if (!int.TryParse(number ?? "21", out var age))
            return Left<string, (int Id, string Name, int Age)>("Age must be an integer.");

        if (age < 0)
            return Left<string, (int Id, string Name, int Age)>("Age must be non-negative.");

        return Right<string, (int Id, string Name, int Age)>((1, sanitizedName, age));
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
        Path.Combine(Path.GetTempPath(), $"fizzbuzz-db-csharp-{Guid.NewGuid():N}.txt");
}
