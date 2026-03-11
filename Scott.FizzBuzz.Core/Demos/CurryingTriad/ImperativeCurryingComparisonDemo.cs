using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!decimal.TryParse(number, out var baseAmount) || baseAmount < 0m)
            {
                _output.WriteLine("Failed: Base amount must be a non-negative decimal.");
                return;
            }

            var tier = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();

            decimal discountRate;
            decimal taxRate;

            if (tier == "standard")
            {
                discountRate = 0.05m;
                taxRate = 0.07m;
            }
            else if (tier == "vip")
            {
                discountRate = 0.15m;
                taxRate = 0.05m;
            }
            else if (tier == "employee")
            {
                discountRate = 0.30m;
                taxRate = 0m;
            }
            else
            {
                _output.WriteLine("Failed: Tier must be one of: standard, vip, employee.");
                return;
            }

            var total = CurryingTriadRules.CalculateTotalNonCurried(baseAmount, discountRate, taxRate);
            _output.WriteLine($"Imperative total: {total:0.00}");
        }, "Imperative Currying Comparison");
}
