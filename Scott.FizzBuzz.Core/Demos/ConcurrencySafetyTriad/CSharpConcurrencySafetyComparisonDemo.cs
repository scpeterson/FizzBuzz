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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!ConcurrencySafetyRules.TryParseIterations(number, out var iterations, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var summary = ConcurrencySafetyRules.ExecuteCSharpAtomic(iterations);
            _output.WriteLine("Result: no lost updates.");
            _output.WriteLine(ConcurrencySafetyRules.FormatSummary(summary));
            _output.WriteLine("C# note: explicit atomic operations are required for shared mutable state.");
        }, "C# Concurrency Safety Comparison");
}
