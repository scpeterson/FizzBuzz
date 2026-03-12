using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.AffMonadTriad;

public class LanguageExtAffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtAffMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtAffMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-aff-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "aff", "monad"];
    public string Description => "LanguageExt Aff composition for asynchronous effects with explicit success/failure values.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Aff Monad Comparison",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, int> ComputeResult(string? name, string? number) =>
        AffMonadRules.ComputeAff(name, number)
            .Run()
            .AsTask()
            .GetAwaiter()
            .GetResult()
            .Match(
                Succ: result => result,
                Fail: error => Left<string, int>($"Aff failure: {error.Message}"));
}
