using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public class ImperativeEitherComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeEitherComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeEitherComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-either-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "either", "monad"];
    public string Description => "Manual error propagation with mutable variables and branch-heavy flow control.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var errorsChecked = 0;

            errorsChecked++;
            if (!decimal.TryParse(number, out var amount))
            {
                _output.WriteLine("Failed: Amount must be a valid decimal value.");
                _output.WriteLine($"Imperative error checks: {errorsChecked}");
                return;
            }

            errorsChecked++;
            if (amount < 1m || amount > 1000m)
            {
                _output.WriteLine("Failed: Amount must be between 1 and 1000.");
                _output.WriteLine($"Imperative error checks: {errorsChecked}");
                return;
            }

            EitherDiscountCode code;
            if (string.IsNullOrWhiteSpace(name))
            {
                code = EitherDiscountCode.None;
            }
            else
            {
                var normalized = name.Trim().ToLowerInvariant();
                errorsChecked++;
                if (normalized == "vip")
                {
                    code = EitherDiscountCode.Vip;
                }
                else if (normalized == "student")
                {
                    code = EitherDiscountCode.Student;
                }
                else if (normalized == "employee")
                {
                    code = EitherDiscountCode.Employee;
                }
                else
                {
                    _output.WriteLine("Failed: Unknown discount code. Use vip, student, or employee.");
                    _output.WriteLine($"Imperative error checks: {errorsChecked}");
                    return;
                }
            }

            var finalAmount = amount * EitherMonadRules.DiscountFactor(code);

            errorsChecked++;
            if (finalAmount < 1m)
            {
                _output.WriteLine("Failed: Final amount is below the minimum charge.");
                _output.WriteLine($"Imperative error checks: {errorsChecked}");
                return;
            }

            _output.WriteLine($"Final amount: {finalAmount:0.00}");
            _output.WriteLine($"Imperative error checks: {errorsChecked}");
        }, "Imperative Either Comparison");
}
