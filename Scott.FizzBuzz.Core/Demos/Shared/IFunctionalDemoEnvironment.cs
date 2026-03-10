using LanguageExt;

namespace Scott.FizzBuzz.Core.Demos.Shared;

public interface IFunctionalDemoEnvironment
{
    Either<string, decimal> ResolveDiscountRate(string tier);
    Either<string, decimal> ResolveTaxRate(string region);
    decimal MaxAutoApproveAmount { get; }
    IReadOnlySet<string> SeedProcessedCommandIds { get; }
}
