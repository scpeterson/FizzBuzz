using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public class CSharpTryOptionMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpTryOptionMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpTryOptionMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-tryoption-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "tryoption", "monad"];
    public string Description => "Plain C# pipeline combining parse failure and optional lookup absence with explicit result values.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!TryOptionMonadRules.TryParseId(number, out var id, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            try
            {
                var value = TryOptionMonadRules.LookupNullable(id);
                _output.WriteLine(value.HasValue
                    ? $"Result: {value.Value:0.##}"
                    : "Failed: No value for id.");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "C# TryOption Monad Comparison");
}
