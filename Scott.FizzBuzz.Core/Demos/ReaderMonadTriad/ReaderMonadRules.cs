namespace Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;

public static class ReaderMonadRules
{
    public static bool TryParseSubtotal(string? input, out decimal subtotal, out string? error)
    {
        if (!decimal.TryParse(input, out subtotal))
        {
            error = "Subtotal must be a valid decimal value.";
            return false;
        }

        if (subtotal is < 1m or > 10000m)
        {
            error = "Subtotal must be between 1 and 10000.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryResolveContext(string? profile, out ReaderPricingContext? context, out string? error)
    {
        context = ReaderMonadSampleData.ResolveProfile(profile);
        if (context is null)
        {
            error = "Unknown pricing profile. Use standard, vip, or intl.";
            return false;
        }

        error = null;
        return true;
    }

    public static decimal ApplyTax(decimal subtotal, ReaderPricingContext context) =>
        subtotal * (1m + context.TaxRate);

    public static decimal AddFee(decimal taxed, ReaderPricingContext context) =>
        taxed + context.ServiceFee;

    public static string FormatTotal(decimal total, ReaderPricingContext context) =>
        $"{context.ProfileName}: total = {total:0.00} {context.Currency}";
}
