using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public static class LanguageExtDomainWorkflowRules
{
    public static Either<string, decimal> ParseAmount(string? number) =>
        DomainWorkflowRules.TryParseAmount(number, out var amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Amount must be a non-negative decimal.");

    public static Either<string, DomainWorkflowRules.Draft> CreateDraft(decimal amount) =>
        Right<string, DomainWorkflowRules.Draft>(DomainWorkflowRules.CreateDraft(amount));

    public static Either<string, DomainWorkflowRules.Authorized> Authorize(
        IFunctionalDemoEnvironment env,
        DomainWorkflowRules.Draft draft) =>
        DomainWorkflowRules.TryAuthorize(env, draft, out var authorized, out var error)
            ? Right<string, DomainWorkflowRules.Authorized>(authorized!)
            : Left<string, DomainWorkflowRules.Authorized>(error ?? "Authorization failed.");

    public static Either<string, DomainWorkflowRules.Packed> Pack(DomainWorkflowRules.Authorized authorized) =>
        Right<string, DomainWorkflowRules.Packed>(DomainWorkflowRules.Pack(authorized));

    public static Either<string, DomainWorkflowRules.Shipped> Ship(DomainWorkflowRules.Packed packed) =>
        Right<string, DomainWorkflowRules.Shipped>(DomainWorkflowRules.Ship(packed));
}
