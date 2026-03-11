using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public class CSharpCompositionRootComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpCompositionRootComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpCompositionRootComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-composition-root";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "composition-root", "triad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var env = new InMemoryFunctionalDemoEnvironment();
            var tier = CompositionRootRules.NormalizeTier(name);
            var region = "us";

            var totalResult =
                from amount in CompositionRootRules.ParseAmount(number)
                from total in CompositionRootRules.QuoteWithInjectedFunctions(
                    amount,
                    env.ResolveDiscountRate,
                    env.ResolveTaxRate,
                    tier is "standard" or "vip" or "employee" ? tier : "standard",
                    region)
                select total;

            totalResult.Match(
                Right: total => _output.WriteLine($"C# composition-root total: {total:0.00}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Composition Root Comparison");
}
