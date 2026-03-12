using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public class LanguageExtTryMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtTryMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtTryMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-try-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "try", "monad"];
    public string Description => "LanguageExt Try monad composition around risky calculations that may throw.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Try Monad Comparison",
            ComputeResult(number),
            (output, result) => output.WriteLine($"Result: {result:0.####}"));

    private static Either<string, decimal> ComputeResult(string? number) =>
        LanguageExtTryMonadRules.ParseInput(number)
            .Bind(value =>
                LanguageExtTryMonadRules.InverseTry(value)
                    .Match(
                        Succ: inverse => Right<string, decimal>(inverse),
                        Fail: ex => Left<string, decimal>(ex.Message)));
}
