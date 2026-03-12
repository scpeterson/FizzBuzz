using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public class ImperativeCurryingComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeCurryingComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeCurryingComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-currying-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "currying", "triad"];
    public string Description => "Imperative calculation flow that repeats parameter wiring instead of using partial application.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!CurryingTriadRules.TryParseBaseAmount(number, out var baseAmount, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!CurryingTriadRules.TryResolveRates(name, out var rates, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var total = CurryingTriadRules.CalculateTotalNonCurried(baseAmount, rates.DiscountRate, rates.TaxRate);
            _output.WriteLine($"Result: total = {total:0.00}");
        }, "Imperative Currying Comparison");
}
