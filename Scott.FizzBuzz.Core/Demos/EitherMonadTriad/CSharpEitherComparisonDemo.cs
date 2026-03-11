using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public class CSharpEitherComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpEitherComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpEitherComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-either-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "either", "monad"];
    public string Description => "C#-only Result emulation shows extra scaffolding needed to mimic Either bind/map behavior.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseAmount(number)
                .Bind(ValidateAmountRange)
                .Bind(amount => ParseDiscountCode(name).Map(code => amount * EitherMonadRules.DiscountFactor(code)))
                .Bind(EnsureMinimumCharge);

            if (result.IsSuccess)
            {
                _output.WriteLine($"Final amount: {result.Value:0.00}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }

            _output.WriteLine("C#/.NET comparison note: we needed a custom Result type and Bind/Map helpers.");
        }, "C# Either Comparison");

    private static CSharpEitherResult<decimal> ParseAmount(string? input) =>
        EitherMonadRules.ParseAmount(input)
            .Match(
                Right: CSharpEitherResult<decimal>.Success,
                Left: CSharpEitherResult<decimal>.Failure);

    private static CSharpEitherResult<decimal> ValidateAmountRange(decimal amount) =>
        EitherMonadRules.ValidateAmountRange(amount)
            .Match(
                Right: CSharpEitherResult<decimal>.Success,
                Left: CSharpEitherResult<decimal>.Failure);

    private static CSharpEitherResult<EitherDiscountCode> ParseDiscountCode(string? code) =>
        EitherMonadRules.ParseDiscountCode(code)
            .Match(
                Right: CSharpEitherResult<EitherDiscountCode>.Success,
                Left: CSharpEitherResult<EitherDiscountCode>.Failure);

    private static CSharpEitherResult<decimal> EnsureMinimumCharge(decimal amount) =>
        EitherMonadRules.EnsureMinimumCharge(amount)
            .Match(
                Right: CSharpEitherResult<decimal>.Success,
                Left: CSharpEitherResult<decimal>.Failure);
}
