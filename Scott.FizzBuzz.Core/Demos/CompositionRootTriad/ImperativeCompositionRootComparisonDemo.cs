using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public class ImperativeCompositionRootComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeCompositionRootComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeCompositionRootComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-composition-root";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "composition-root", "triad"];
    public string Description => "Imperative composition root flow with manual dependency lookups and branch-heavy orchestration.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!CompositionRootRules.TryParseAmount(number, out var amount, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var env = new InMemoryFunctionalDemoEnvironment();
            var tier = CompositionRootRules.NormalizeTier(name);
            var region = "us";

            var normalizedTier = tier is "vip" or "employee" or "standard" ? tier : "standard";
            if (!env.TryResolveDiscountRate(normalizedTier, out var discountRate, out var discountError))
            {
                _output.WriteLine($"Failed: {discountError}");
                return;
            }

            if (!env.TryResolveTaxRate(region, out var taxRate, out var taxError))
            {
                _output.WriteLine($"Failed: {taxError}");
                return;
            }

            var total = CompositionRootRules.CalculateTotal(amount, discountRate, taxRate);
            _output.WriteLine($"Result: total = {total:0.00}");
        }, "Imperative Composition Root Comparison");
}
