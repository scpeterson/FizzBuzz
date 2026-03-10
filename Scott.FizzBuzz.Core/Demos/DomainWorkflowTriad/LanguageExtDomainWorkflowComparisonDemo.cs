using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public class LanguageExtDomainWorkflowComparisonDemo : IDemo
{
    public string Key => "langext-domain-workflow";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "domain-modeling", "triad"];

    public Either<string, Unit> Run(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();

        var result =
            from amount in DomainWorkflowRules.ParseAmount(number)
            from draft in DomainWorkflowRules.CreateDraft(amount)
            from authorized in DomainWorkflowRules.Authorize(env, draft)
            from packed in DomainWorkflowRules.Pack(authorized)
            from shipped in DomainWorkflowRules.Ship(packed)
            select shipped;

        return result.Map(_ => unit);
    }
}
