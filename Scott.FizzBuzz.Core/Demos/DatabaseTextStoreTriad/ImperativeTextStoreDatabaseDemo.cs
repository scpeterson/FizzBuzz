using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DatabaseTextStoreTriad;

public class ImperativeTextStoreDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeTextStoreDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeTextStoreDatabaseDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-db-text-store";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "database", "io", "text-store"];
    public string Description => "Imperative file-based persistence with inline parsing and mutation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var filePath = BuildTempPath();
            try
            {
                var userName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();
                var age = int.Parse(number ?? "21");

                var rows = new List<PersonRecord>();
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length != 3)
                            continue;

                        if (!int.TryParse(parts[0], out var id) || !int.TryParse(parts[2], out var existingAge))
                            continue;

                        rows.Add(new PersonRecord(id, parts[1], existingAge));
                    }
                }

                var nextId = rows.Count == 0 ? 1 : rows.Max(row => row.Id) + 1;
                rows.Add(new PersonRecord(nextId, userName, age));

                var linesToWrite = rows
                    .OrderBy(row => row.Id)
                    .Select(row => $"{row.Id}|{row.Name}|{row.Age}")
                    .ToArray();

                File.WriteAllLines(filePath, linesToWrite);

                var persistedLine = File.ReadAllLines(filePath).Last();
                _output.WriteLine($"Saved row: {persistedLine}");
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }, "Imperative Text-Store Database");

    private static string BuildTempPath() =>
        Path.Combine(Path.GetTempPath(), $"fizzbuzz-db-imperative-{Guid.NewGuid():N}.txt");
}
