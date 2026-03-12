namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public static class CurryingTriadRules
{
    public static bool TryParseBaseAmount(string? number, out decimal amount, out string? error)
    {
        if (decimal.TryParse(number, out amount) && amount >= 0m)
        {
            error = null;
            return true;
        }

        error = "Base amount must be a non-negative decimal.";
        return false;
    }

    public static bool TryResolveRates(string? tier, out (decimal DiscountRate, decimal TaxRate) rates, out string? error)
    {
        var normalized = string.IsNullOrWhiteSpace(tier)
            ? "standard"
            : tier.Trim().ToLowerInvariant();

        switch (normalized)
        {
            case "standard":
                rates = (0.05m, 0.07m);
                error = null;
                return true;
            case "vip":
                rates = (0.15m, 0.05m);
                error = null;
                return true;
            case "employee":
                rates = (0.30m, 0.00m);
                error = null;
                return true;
            default:
                rates = default;
                error = "Tier must be one of: standard, vip, employee.";
                return false;
        }
    }

    public static decimal CalculateTotalNonCurried(decimal baseAmount, decimal discountRate, decimal taxRate) =>
        decimal.Round(baseAmount * (1m - discountRate) * (1m + taxRate), 2, MidpointRounding.AwayFromZero);

    public static Func<decimal, Func<decimal, Func<decimal, decimal>>> CurriedTotal =>
        baseAmount => discountRate => taxRate =>
            CalculateTotalNonCurried(baseAmount, discountRate, taxRate);
}
