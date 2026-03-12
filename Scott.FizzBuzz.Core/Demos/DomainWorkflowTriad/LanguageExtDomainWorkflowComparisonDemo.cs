using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public class LanguageExtDomainWorkflowComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtDomainWorkflowComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtDomainWorkflowComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-domain-workflow";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "domain-modeling", "triad"];
    public string Description => "LanguageExt domain workflow with explicit state transitions composed through Either.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Domain Workflow Comparison",
            ComputeResult(name, number),
            (output, state) => output.WriteLine($"Result: {DomainWorkflowRules.Render(state)}"));

    private static Either<string, DomainWorkflowRules.FulfillmentState> ComputeResult(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();

        return
            from amount in LanguageExtDomainWorkflowRules.ParseAmount(number)
            from draft in LanguageExtDomainWorkflowRules.CreateDraft(amount)
            from authorized in LanguageExtDomainWorkflowRules.Authorize(env, draft)
            from packed in LanguageExtDomainWorkflowRules.Pack(authorized)
            from shipped in LanguageExtDomainWorkflowRules.Ship(packed)
            select (DomainWorkflowRules.FulfillmentState)shipped;
    }
}
