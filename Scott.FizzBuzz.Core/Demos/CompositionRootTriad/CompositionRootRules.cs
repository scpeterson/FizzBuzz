using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public static class CompositionRootRules
{
    public static Either<string, decimal> ParseAmount(string? number) =>
        decimal.TryParse(number, out var amount) && amount >= 0m
            ? Right<string, decimal>(amount)
            : Left<string, decimal>("Amount must be a non-negative decimal.");

    public static string NormalizeTier(string? tier) =>
        string.IsNullOrWhiteSpace(tier) ? "standard" : tier.Trim().ToLowerInvariant();

    public static decimal CalculateTotal(decimal amount, decimal discountRate, decimal taxRate) =>
        decimal.Round(amount * (1m - discountRate) * (1m + taxRate), 2, MidpointRounding.AwayFromZero);

    public static Either<string, decimal> QuoteWithInjectedFunctions(
        decimal amount,
        Func<string, Either<string, decimal>> resolveDiscountRate,
        Func<string, Either<string, decimal>> resolveTaxRate,
        string tier,
        string region) =>
        from discount in resolveDiscountRate(tier)
        from tax in resolveTaxRate(region)
        select CalculateTotal(amount, discount, tax);

    public static Reader<IFunctionalDemoEnvironment, Either<string, decimal>> QuoteReader(decimal amount, string tier, string region) =>
        from env in ask<IFunctionalDemoEnvironment>()
        select QuoteWithInjectedFunctions(amount, env.ResolveDiscountRate, env.ResolveTaxRate, tier, region);
}
