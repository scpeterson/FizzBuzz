namespace Scott.FizzBuzz.Core.Demos.Shared;

public interface IFunctionalDemoEnvironment
{
    bool TryResolveDiscountRate(string tier, out decimal rate, out string? error);
    bool TryResolveTaxRate(string region, out decimal rate, out string? error);
    decimal MaxAutoApproveAmount { get; }
    IReadOnlySet<string> SeedProcessedCommandIds { get; }
}
