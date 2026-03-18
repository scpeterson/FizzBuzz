using Scott.FunctionalProgrammingTriads.Core.Interfaces;
using static Scott.FunctionalProgrammingTriads.Core.OutputUtilities;

namespace Scott.FunctionalProgrammingTriads.Core.Demos.DatabaseTextStoreTriad;

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
    public string Description => "C# functional core with file IO isolated to explicit boundaries and plain result types.";

    public DemoExecutionResult Run(string? name, string? number)
    {
        var filePath = BuildTempPath();

        try
        {
            var result = SaveAndReadBack(filePath, name, number);
            if (!result.IsSuccess)
                return DemoExecutionResult.Failure(result.Error ?? "Database operation failed.");

            return ExecuteWithSpacing(
                _output,
                () => _output.WriteLine($"Saved row: {SerializeLine(result.Value)}"),
                "C# Functional Text-Store Database");
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    internal static DatabaseResult<PersonRecord> SaveAndReadBack(string filePath, string? name, string? number) =>
        ParseInput(name, number)
            .Bind(input =>
                LoadRecords(filePath)
                    .Map(records => Upsert(records, new PersonRecord(input.Id, input.Name, input.Age)))
                    .Bind(updated => SaveRecords(filePath, updated)
                        .Bind(_ => FindById(updated, input.Id))));

    private static DatabaseResult<(int Id, string Name, int Age)> ParseInput(string? name, string? number)
    {
        var sanitizedName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();

        if (!int.TryParse(number ?? "21", out var age))
            return DatabaseResult<(int Id, string Name, int Age)>.Failure("Age must be an integer.");

        if (age < 0)
            return DatabaseResult<(int Id, string Name, int Age)>.Failure("Age must be non-negative.");

        return DatabaseResult<(int Id, string Name, int Age)>.Success((1, sanitizedName, age));
    }

    private static DatabaseResult<List<PersonRecord>> LoadRecords(string filePath)
    {
        var content = File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : string.Empty;

        return ParseRecords(content);
    }

    private static DatabaseResult<Unit> SaveRecords(string filePath, IReadOnlyCollection<PersonRecord> records)
    {
        File.WriteAllText(filePath, Serialize(records));
        return DatabaseResult<Unit>.Success(Unit.Value);
    }

    private static string BuildTempPath() =>
        Path.Combine(Path.GetTempPath(), $"functional-programming-triads-db-csharp-{Guid.NewGuid():N}.txt");

    private static DatabaseResult<List<PersonRecord>> ParseRecords(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return DatabaseResult<List<PersonRecord>>.Success([]);

        var rows = new List<PersonRecord>();
        var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parsed = ParseLine(line);
            if (!parsed.IsSuccess)
                return DatabaseResult<List<PersonRecord>>.Failure(parsed.Error ?? $"Invalid row format: '{line}'");

            rows.Add(parsed.Value);
        }

        return DatabaseResult<List<PersonRecord>>.Success(rows);
    }

    private static DatabaseResult<PersonRecord> ParseLine(string line)
    {
        var parts = line.Split('|', StringSplitOptions.None);
        if (parts.Length != 3)
            return DatabaseResult<PersonRecord>.Failure($"Invalid row format: '{line}'");

        if (!int.TryParse(parts[0], out var id))
            return DatabaseResult<PersonRecord>.Failure($"Invalid id value in row: '{line}'");

        var name = parts[1].Trim();
        if (string.IsNullOrWhiteSpace(name))
            return DatabaseResult<PersonRecord>.Failure($"Name is required in row: '{line}'");

        if (!int.TryParse(parts[2], out var age))
            return DatabaseResult<PersonRecord>.Failure($"Invalid age value in row: '{line}'");

        return DatabaseResult<PersonRecord>.Success(new PersonRecord(id, name, age));
    }

    private static List<PersonRecord> Upsert(IEnumerable<PersonRecord> rows, PersonRecord candidate) =>
        rows.Where(row => row.Id != candidate.Id)
            .Append(candidate)
            .OrderBy(row => row.Id)
            .ToList();

    private static DatabaseResult<PersonRecord> FindById(IEnumerable<PersonRecord> rows, int id)
    {
        foreach (var row in rows)
        {
            if (row.Id == id)
                return DatabaseResult<PersonRecord>.Success(row);
        }

        return DatabaseResult<PersonRecord>.Failure($"Record '{id}' was not found after save.");
    }

    private static string Serialize(IEnumerable<PersonRecord> rows) =>
        string.Join(Environment.NewLine, rows.Select(SerializeLine));

    private static string SerializeLine(PersonRecord record) =>
        $"{record.Id}|{record.Name}|{record.Age}";

    internal readonly record struct Unit
    {
        public static Unit Value => new();
    }

    internal readonly record struct DatabaseResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DatabaseResult<T> Success(T value) => new(true, value, null);
        public static DatabaseResult<T> Failure(string error) => new(false, default!, error);

        public DatabaseResult<TNext> Bind<TNext>(Func<T, DatabaseResult<TNext>> next) =>
            IsSuccess
                ? next(Value)
                : DatabaseResult<TNext>.Failure(Error ?? "Database operation failed.");

        public DatabaseResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess
                ? DatabaseResult<TNext>.Success(map(Value))
                : DatabaseResult<TNext>.Failure(Error ?? "Database operation failed.");
    }
}
