using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public class LanguageExtCompositionRootComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtCompositionRootComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtCompositionRootComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-composition-root";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "composition-root", "triad"];
    public string Description => "LanguageExt composition root using Reader-style environment access and Either-based flow.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Composition Root Comparison",
            ComputeResult(name, number),
            (output, total) => output.WriteLine($"Result: total = {total:0.00}"));

    private static Either<string, decimal> ComputeResult(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();
        var tier = CompositionRootRules.NormalizeTier(name);
        var normalizedTier = tier is "standard" or "vip" or "employee" ? tier : "standard";

        return LanguageExtCompositionRootRules.ParseAmount(number)
            .Bind(amount =>
            {
                var readerResult = LanguageExtCompositionRootRules.QuoteReader(amount, normalizedTier, "us").Run(env);
                return readerResult.Match(
                    Succ: total => total,
                    Fail: error => Left<string, decimal>($"Reader failure: {error.Message}"));
            });
    }
}
