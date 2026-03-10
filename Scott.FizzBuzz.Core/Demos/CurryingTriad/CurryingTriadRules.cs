using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public static class CurryingTriadRules
{
    public static Either<string, decimal> ParseBaseAmount(string? number) =>
        decimal.TryParse(number, out var amount) && amount >= 0m
            ? Right<string, decimal>(amount)
            : Left<string, decimal>("Base amount must be a non-negative decimal.");

    public static Either<string, (decimal DiscountRate, decimal TaxRate)> ResolveRates(string? tier)
    {
        var normalized = string.IsNullOrWhiteSpace(tier)
            ? "standard"
            : tier.Trim().ToLowerInvariant();

        return normalized switch
        {
            "standard" => Right<string, (decimal, decimal)>((0.05m, 0.07m)),
            "vip" => Right<string, (decimal, decimal)>((0.15m, 0.05m)),
            "employee" => Right<string, (decimal, decimal)>((0.30m, 0.00m)),
            _ => Left<string, (decimal, decimal)>("Tier must be one of: standard, vip, employee.")
        };
    }

    public static decimal CalculateTotalNonCurried(decimal baseAmount, decimal discountRate, decimal taxRate) =>
        decimal.Round(baseAmount * (1m - discountRate) * (1m + taxRate), 2, MidpointRounding.AwayFromZero);

    public static Func<decimal, Func<decimal, Func<decimal, decimal>>> CurriedTotal =>
        baseAmount => discountRate => taxRate =>
            CalculateTotalNonCurried(baseAmount, discountRate, taxRate);
}
