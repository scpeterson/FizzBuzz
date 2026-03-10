using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.Shared;

public sealed class InMemoryFunctionalDemoEnvironment : IFunctionalDemoEnvironment
{
    private static readonly IReadOnlyDictionary<string, decimal> DiscountRates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
    {
        ["standard"] = 0.05m,
        ["vip"] = 0.15m,
        ["employee"] = 0.30m
    };

    private static readonly IReadOnlyDictionary<string, decimal> TaxRates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
    {
        ["us"] = 0.07m,
        ["eu"] = 0.20m,
        ["none"] = 0.00m
    };

    public decimal MaxAutoApproveAmount => 250m;

    public IReadOnlySet<string> SeedProcessedCommandIds { get; } = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "cmd-processed"
    };

    public Either<string, decimal> ResolveDiscountRate(string tier) =>
        DiscountRates.TryGetValue(tier, out var rate)
            ? Right<string, decimal>(rate)
            : Left<string, decimal>($"Unknown tier '{tier}'.");

    public Either<string, decimal> ResolveTaxRate(string region) =>
        TaxRates.TryGetValue(region, out var rate)
            ? Right<string, decimal>(rate)
            : Left<string, decimal>($"Unknown region '{region}'.");
}
