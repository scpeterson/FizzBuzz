using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public static class LanguageExtCompositionRootRules
{
    public static Either<string, decimal> ParseAmount(string? number) =>
        CompositionRootRules.TryParseAmount(number, out var amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Amount must be a non-negative decimal.");

    public static Either<string, decimal> QuoteWithInjectedFunctions(
        decimal amount,
        Func<string, Either<string, decimal>> resolveDiscountRate,
        Func<string, Either<string, decimal>> resolveTaxRate,
        string tier,
        string region) =>
        from discount in resolveDiscountRate(tier)
        from tax in resolveTaxRate(region)
        select CompositionRootRules.CalculateTotal(amount, discount, tax);

    public static Reader<IFunctionalDemoEnvironment, Either<string, decimal>> QuoteReader(decimal amount, string tier, string region) =>
        from env in ask<IFunctionalDemoEnvironment>()
        select QuoteWithInjectedFunctions(
            amount,
            candidateTier => env.TryResolveDiscountRate(candidateTier, out var discountRate, out var discountError)
                ? Right<string, decimal>(discountRate)
                : Left<string, decimal>(discountError ?? $"Unknown tier '{candidateTier}'."),
            candidateRegion => env.TryResolveTaxRate(candidateRegion, out var taxRate, out var taxError)
                ? Right<string, decimal>(taxRate)
                : Left<string, decimal>(taxError ?? $"Unknown region '{candidateRegion}'."),
            tier,
            region);
}
