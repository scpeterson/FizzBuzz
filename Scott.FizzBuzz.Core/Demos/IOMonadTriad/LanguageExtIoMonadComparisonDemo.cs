using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public class LanguageExtIoMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtIoMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtIoMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-io-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "io", "monad"];
    public string Description => "LanguageExt IO-style composition using Eff (v4 equivalent) for effectful workflow sequencing.";

    public DemoExecutionResult Run(string? name, string? number)
    {
        var program =
            from profile in Eff(() => LanguageExtIoMonadRules.ResolveProfile(name))
            from weight in Eff(() => LanguageExtIoMonadRules.ParseWeight(number))
            select
                from p in profile
                from w in weight
                select IoMonadRules.CalculateQuote(w, p);

        var result = program
            .Run()
            .Match(
                Succ: success => success,
                Fail: error => Left<string, decimal>($"IO effect failure: {error.Message}"));

        return FunctionalDemoOutput.Render(
            _output,
            "LanguageExt IO Monad Comparison",
            result,
            (output, quote) => output.WriteLine($"Result: quote = {quote:0.00}"));
    }
}
