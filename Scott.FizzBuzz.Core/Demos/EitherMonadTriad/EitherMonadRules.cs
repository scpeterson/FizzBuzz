namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public static class EitherMonadRules
{
    public static bool TryParseAmount(string? input, out decimal amount, out string? error)
    {
        if (decimal.TryParse(input, out amount))
        {
            error = null;
            return true;
        }

        error = "Amount must be a valid decimal value.";
        return false;
    }

    public static bool TryValidateAmountRange(decimal amount, out string? error)
    {
        if (amount is >= 1m and <= 1000m)
        {
            error = null;
            return true;
        }

        error = "Amount must be between 1 and 1000.";
        return false;
    }

    public static bool TryParseDiscountCode(string? code, out EitherDiscountCode parsedCode, out string? error)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            parsedCode = EitherDiscountCode.None;
            error = null;
            return true;
        }

        var normalized = code.Trim().ToLowerInvariant();
        switch (normalized)
        {
            case "vip":
                parsedCode = EitherDiscountCode.Vip;
                error = null;
                return true;
            case "student":
                parsedCode = EitherDiscountCode.Student;
                error = null;
                return true;
            case "employee":
                parsedCode = EitherDiscountCode.Employee;
                error = null;
                return true;
            case "scott":
                parsedCode = EitherDiscountCode.None;
                error = null;
                return true;
            default:
                parsedCode = EitherDiscountCode.None;
                error = "Unknown discount code. Use vip, student, or employee.";
                return false;
        }
    }

    public static decimal DiscountFactor(EitherDiscountCode code) =>
        code switch
        {
            EitherDiscountCode.None => 1.00m,
            EitherDiscountCode.Vip => 0.80m,
            EitherDiscountCode.Student => 0.90m,
            EitherDiscountCode.Employee => 0.75m,
            _ => 1.00m
        };

    public static bool TryEnsureMinimumCharge(decimal amount, out string? error)
    {
        if (amount >= 1m)
        {
            error = null;
            return true;
        }

        error = "Final amount is below the minimum charge.";
        return false;
    }
}
