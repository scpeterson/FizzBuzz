namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public static class TryMonadRules
{
    public static bool TryParseInput(string? value, out decimal parsed, out string? error)
    {
        if (decimal.TryParse(value, out parsed))
        {
            error = null;
            return true;
        }

        error = "Input must be numeric.";
        return false;
    }

    public static decimal RiskyInverse(decimal value)
    {
        if (value == 0m)
        {
            throw new DivideByZeroException("Cannot invert zero.");
        }

        return 1m / value;
    }
}
