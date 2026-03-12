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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseAmount(number)
                .Bind(ValidateAmountRange)
                .Bind(amount => ParseDiscountCode(name).Map(code => amount * EitherMonadRules.DiscountFactor(code)))
                .Bind(EnsureMinimumCharge);

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: final amount = {result.Value:0.00}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }

            _output.WriteLine("C#/.NET comparison note: we needed a custom Result type and Bind/Map helpers.");
        }, "C# Either Comparison");

    private static CSharpEitherResult<decimal> ParseAmount(string? input) =>
        EitherMonadRules.TryParseAmount(input, out var amount, out var error)
            ? CSharpEitherResult<decimal>.Success(amount)
            : CSharpEitherResult<decimal>.Failure(error ?? "Amount must be a valid decimal value.");

    private static CSharpEitherResult<decimal> ValidateAmountRange(decimal amount) =>
        EitherMonadRules.TryValidateAmountRange(amount, out var error)
            ? CSharpEitherResult<decimal>.Success(amount)
            : CSharpEitherResult<decimal>.Failure(error ?? "Amount must be between 1 and 1000.");

    private static CSharpEitherResult<EitherDiscountCode> ParseDiscountCode(string? code) =>
        EitherMonadRules.TryParseDiscountCode(code, out var parsedCode, out var error)
            ? CSharpEitherResult<EitherDiscountCode>.Success(parsedCode)
            : CSharpEitherResult<EitherDiscountCode>.Failure(error ?? "Unknown discount code. Use vip, student, or employee.");

    private static CSharpEitherResult<decimal> EnsureMinimumCharge(decimal amount) =>
        EitherMonadRules.TryEnsureMinimumCharge(amount, out var error)
            ? CSharpEitherResult<decimal>.Success(amount)
            : CSharpEitherResult<decimal>.Failure(error ?? "Final amount is below the minimum charge.");
}
