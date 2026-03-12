using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public static class LanguageExtCurryingTriadRules
{
    public static Either<string, decimal> ParseBaseAmount(string? number) =>
        CurryingTriadRules.TryParseBaseAmount(number, out var amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Base amount must be a non-negative decimal.");

    public static Either<string, (decimal DiscountRate, decimal TaxRate)> ResolveRates(string? tier) =>
        CurryingTriadRules.TryResolveRates(tier, out var rates, out var error)
            ? Right<string, (decimal, decimal)>(rates)
            : Left<string, (decimal, decimal)>(error ?? "Tier must be one of: standard, vip, employee.");
}
