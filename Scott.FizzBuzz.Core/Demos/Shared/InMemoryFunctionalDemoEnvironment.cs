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

    public bool TryResolveDiscountRate(string tier, out decimal rate, out string? error)
    {
        if (DiscountRates.TryGetValue(tier, out rate))
        {
            error = null;
            return true;
        }

        error = $"Unknown tier '{tier}'.";
        return false;
    }

    public bool TryResolveTaxRate(string region, out decimal rate, out string? error)
    {
        if (TaxRates.TryGetValue(region, out rate))
        {
            error = null;
            return true;
        }

        error = $"Unknown region '{region}'.";
        return false;
    }
}
