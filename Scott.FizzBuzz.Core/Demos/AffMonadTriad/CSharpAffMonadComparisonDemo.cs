using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.AffMonadTriad;

public class CSharpAffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpAffMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpAffMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-aff-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "aff", "monad"];
    public string Description => "Plain C# result composition around async delay work, showing the extra scaffolding before Aff.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from count in AffMonadRules.ParseCount(number)
                from delay in AffMonadRules.ResolveDelayMs(name)
                select (count, delay);

            result.Match(
                Right: tuple =>
                {
                    Task.Delay(tuple.delay).GetAwaiter().GetResult();
                    _output.WriteLine($"Result: {tuple.count * 2}");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Aff Monad Comparison");
}
