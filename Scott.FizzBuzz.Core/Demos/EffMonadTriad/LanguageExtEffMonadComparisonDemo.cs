using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EffMonadTriad;

public class LanguageExtEffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtEffMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtEffMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-eff-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "eff", "monad"];
    public string Description => "LanguageExt Eff composition for synchronous effects with pure transforms at the boundary.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Eff Monad Comparison",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: {result:0.##}"));

    private static Either<string, decimal> ComputeResult(string? name, string? number) =>
        EffMonadRules.ComputeEff(name, number)
            .Run()
            .Match(
                Succ: result => result,
                Fail: error => Left<string, decimal>($"Eff failure: {error.Message}"));
}
