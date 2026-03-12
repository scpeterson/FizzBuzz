using Scott.FizzBuzz.Core.Demos.Shared;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public static class DomainWorkflowRules
{
    public abstract record FulfillmentState(decimal Amount);
    public sealed record Draft(decimal Amount) : FulfillmentState(Amount);
    public sealed record Authorized(decimal Amount) : FulfillmentState(Amount);
    public sealed record Packed(decimal Amount) : FulfillmentState(Amount);
    public sealed record Shipped(decimal Amount, DateOnly ShipDate) : FulfillmentState(Amount);

    public static bool TryParseAmount(string? number, out decimal amount, out string? error)
    {
        if (decimal.TryParse(number, out amount) && amount >= 0m)
        {
            error = null;
            return true;
        }

        error = "Amount must be a non-negative decimal.";
        return false;
    }

    public static Draft CreateDraft(decimal amount) => new(amount);

    public static bool TryAuthorize(
        IFunctionalDemoEnvironment env,
        Draft draft,
        out Authorized? authorized,
        out string? error)
    {
        if (draft.Amount <= env.MaxAutoApproveAmount)
        {
            authorized = new Authorized(draft.Amount);
            error = null;
            return true;
        }

        authorized = null;
        error = $"Amount {draft.Amount:0.00} exceeds auto-approval limit {env.MaxAutoApproveAmount:0.00}.";
        return false;
    }

    public static Packed Pack(Authorized authorized) => new(authorized.Amount);

    public static Shipped Ship(Packed packed) => new(packed.Amount, DateOnly.FromDateTime(DateTime.UtcNow));

    public static string Render(FulfillmentState state) => state switch
    {
        Draft => "Draft",
        Authorized => "Authorized",
        Packed => "Packed",
        Shipped shipped => $"Shipped ({shipped.ShipDate:yyyy-MM-dd})",
        _ => "Unknown"
    };
}
