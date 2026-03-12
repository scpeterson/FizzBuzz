namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public static class CompositionRootRules
{
    public static bool TryParseAmount(string? number, out decimal amount, out string? error)
    {
        if (decimal.TryParse(number, out amount) && amount >= 0m)
        {
            error = null;
            return true;
        }

        error = "Amount must be a non-negative decimal.";
        return false;
    }

    public static string NormalizeTier(string? tier) =>
        string.IsNullOrWhiteSpace(tier) ? "standard" : tier.Trim().ToLowerInvariant();

    public static decimal CalculateTotal(decimal amount, decimal discountRate, decimal taxRate) =>
        decimal.Round(amount * (1m - discountRate) * (1m + taxRate), 2, MidpointRounding.AwayFromZero);
}
