using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public class CSharpCurryingComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpCurryingComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpCurryingComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => "csharp-currying-comparison";
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "currying", "triad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var amountEither = CurryingTriadRules.ParseBaseAmount(number);
            var ratesEither = CurryingTriadRules.ResolveRates(name);

            if (amountEither.IsLeft || ratesEither.IsLeft)
            {
                var message = amountEither.IsLeft
                    ? amountEither.LeftToList()[0]
                    : ratesEither.LeftToList()[0];

                _output.WriteLine($"Failed: {message}");
                return;
            }

            var amount = amountEither.RightToList()[0];
            var rates = ratesEither.RightToList()[0];

            var applyBase = CurryingTriadRules.CurriedTotal(amount);
            var applyDiscount = applyBase(rates.DiscountRate);
            var total = applyDiscount(rates.TaxRate);

            _output.WriteLine($"C# curried total: {total:0.00}");
        }, "C# Currying Comparison");
}
