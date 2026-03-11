using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.AsyncEffTriad;

public class CSharpAsyncCompositionDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpAsyncCompositionDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpAsyncCompositionDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-async-composition";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "async", "effects"];
    public string Description => "C# functional equivalent: explicit error channel with async composition.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ComposeAsync(number ?? "10").GetAwaiter().GetResult();
            result.Match(
                Right: value => _output.WriteLine($"Result: {value}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Eff/Aff Equivalent Workflow");

    private static async Task<Either<string, int>> ComposeAsync(string input)
    {
        var parsed = Parse(input);

        return await parsed.Match(
            Right: async value =>
            {
                var doubled = value * 2;
                var finalValue = await AddTenAsync(doubled);
                return Right<string, int>(finalValue);
            },
            Left: error => Task.FromResult(Left<string, int>(error)));
    }

    private static Either<string, int> Parse(string input) =>
        int.TryParse(input, out var parsed)
            ? Right<string, int>(parsed)
            : Left<string, int>("Input must be an integer.");

    private static async Task<int> AddTenAsync(int value)
    {
        await Task.Delay(10);
        return value + 10;
    }
}
