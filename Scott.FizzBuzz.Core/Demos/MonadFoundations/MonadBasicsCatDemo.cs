using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using Scott.FizzBuzz.Core.MonadBasics;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.MonadFoundations;

public class MonadBasicsCatDemo : IDemo
{
    private readonly IOutput _output;
    private static readonly IReadOnlyDictionary<string, Cat> Cats = new Dictionary<string, Cat>(StringComparer.OrdinalIgnoreCase)
    {
        ["luna"] = new Cat("Luna", true),
        ["milo"] = new Cat("Milo", null),
        ["void"] = new Cat("Void", false)
    };

    public MonadBasicsCatDemo() : this(new ConsoleOutput())
    {
    }

    public MonadBasicsCatDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "monad-basics-cat";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "monads", "option", "either", "comparison", "beginner"];
    public string Description => "Imperative null checks versus Option/Either pipeline on the same cat lookup.";

    public DemoExecutionResult Run(string? name, string? _) =>
        ExecuteWithSpacing(_output, () =>
        {
            var query = string.IsNullOrWhiteSpace(name) ? "milo" : name;

            _output.WriteLine("=== Imperative null + nested conditionals ===");
            _output.WriteLine(GetCatStatusImperative(query));

            _output.WriteLine(string.Empty);
            _output.WriteLine("=== Functional Option/Either pipeline ===");
            GetCatStatusFunctional(query).Match(
                Right: status => _output.WriteLine(status),
                Left: error => _output.WriteLine(error));
        }, "Monad Basics with Option/Either");

    private static string GetCatStatusImperative(string name)
    {
        if (!Cats.TryGetValue(name, out var cat))
            return $"No cat found for '{name}'.";

        if (!cat.IsAlive.HasValue)
            return $"{cat.Name}: state is unknown.";

        return cat.IsAlive.Value
            ? $"{cat.Name}: alive."
            : $"{cat.Name}: dead.";
    }

    private static Either<string, string> GetCatStatusFunctional(string name) =>
        FindCat(name)
            .ToEither($"No cat found for '{name}'.")
            .Bind(cat => Optional(cat.IsAlive)
                .ToEither($"{cat.Name}: state is unknown.")
                .Map(alive => alive ? $"{cat.Name}: alive." : $"{cat.Name}: dead."));

    private static Option<Cat> FindCat(string name) =>
        Cats.TryGetValue(name, out var cat)
            ? Some(cat)
            : None;
}
