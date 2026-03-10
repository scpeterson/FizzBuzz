using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public static class DomainWorkflowRules
{
    public abstract record FulfillmentState(decimal Amount);
    public sealed record Draft(decimal Amount) : FulfillmentState(Amount);
    public sealed record Authorized(decimal Amount) : FulfillmentState(Amount);
    public sealed record Packed(decimal Amount) : FulfillmentState(Amount);
    public sealed record Shipped(decimal Amount, DateOnly ShipDate) : FulfillmentState(Amount);

    public static Either<string, decimal> ParseAmount(string? number) =>
        decimal.TryParse(number, out var amount) && amount >= 0m
            ? Right<string, decimal>(amount)
            : Left<string, decimal>("Amount must be a non-negative decimal.");

    public static Either<string, Draft> CreateDraft(decimal amount) => Right<string, Draft>(new Draft(amount));

    public static Either<string, Authorized> Authorize(IFunctionalDemoEnvironment env, Draft draft) =>
        draft.Amount <= env.MaxAutoApproveAmount
            ? Right<string, Authorized>(new Authorized(draft.Amount))
            : Left<string, Authorized>($"Amount {draft.Amount:0.00} exceeds auto-approval limit {env.MaxAutoApproveAmount:0.00}.");

    public static Either<string, Packed> Pack(Authorized authorized) => Right<string, Packed>(new Packed(authorized.Amount));

    public static Either<string, Shipped> Ship(Packed packed) =>
        Right<string, Shipped>(new Shipped(packed.Amount, DateOnly.FromDateTime(DateTime.UtcNow)));

    public static string Render(FulfillmentState state) => state switch
    {
        Draft => "Draft",
        Authorized => "Authorized",
        Packed => "Packed",
        Shipped shipped => $"Shipped ({shipped.ShipDate:yyyy-MM-dd})",
        _ => "Unknown"
    };
}
