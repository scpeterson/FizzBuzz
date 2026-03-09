using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public static class TryMonadRules
{
    public static Either<string, decimal> ParseInput(string? value) =>
        decimal.TryParse(value, out var parsed)
            ? Right<string, decimal>(parsed)
            : Left<string, decimal>("Input must be numeric.");

    public static decimal RiskyInverse(decimal value)
    {
        if (value == 0m)
        {
            throw new DivideByZeroException("Cannot invert zero.");
        }

        return 1m / value;
    }

    public static Try<decimal> InverseTry(decimal value) => Try(() => RiskyInverse(value));
}
