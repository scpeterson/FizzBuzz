using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public class CSharpConcurrencySafetyComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpConcurrencySafetyComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpConcurrencySafetyComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-concurrency-safety-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "concurrency", "safety"];
    public string Description => "Atomic increment boundary using BCL concurrency primitives to preserve updates.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ConcurrencySafetyRules.ParseIterations(number)
                .Map(ConcurrencySafetyRules.ExecuteCSharpAtomic);

            result.Match(
                Right: summary =>
                {
                    _output.WriteLine(ConcurrencySafetyRules.FormatSummary(summary));
                    _output.WriteLine("C# note: explicit atomic operations are required for shared mutable state.");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Concurrency Safety Comparison");
}
