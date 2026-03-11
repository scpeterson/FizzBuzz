using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!decimal.TryParse(number, out var amount) || amount < 0m)
            {
                _output.WriteLine("Failed: Amount must be a non-negative decimal.");
                return;
            }

            var env = new InMemoryFunctionalDemoEnvironment();
            var tier = CompositionRootRules.NormalizeTier(name);
            var region = "us";

            Either<string, decimal> discountRate;
            if (tier == "vip" || tier == "employee" || tier == "standard")
            {
                discountRate = env.ResolveDiscountRate(tier);
            }
            else
            {
                discountRate = env.ResolveDiscountRate("standard");
            }

            var totalResult =
                from discount in discountRate
                from tax in env.ResolveTaxRate(region)
                select CompositionRootRules.CalculateTotal(amount, discount, tax);

            totalResult.Match(
                Right: total => _output.WriteLine($"Imperative composition-root total: {total:0.00}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "Imperative Composition Root Comparison");
}
