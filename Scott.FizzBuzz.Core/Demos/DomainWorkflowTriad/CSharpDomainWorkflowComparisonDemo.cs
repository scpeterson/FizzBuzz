using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public class CSharpDomainWorkflowComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpDomainWorkflowComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpDomainWorkflowComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-domain-workflow";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "domain-modeling", "triad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var env = new InMemoryFunctionalDemoEnvironment();

            var stateResult =
                from amount in DomainWorkflowRules.ParseAmount(number)
                from draft in DomainWorkflowRules.CreateDraft(amount)
                from authorized in DomainWorkflowRules.Authorize(env, draft)
                from packed in DomainWorkflowRules.Pack(authorized)
                from shipped in DomainWorkflowRules.Ship(packed)
                select (DomainWorkflowRules.FulfillmentState)shipped;

            stateResult.Match(
                Right: state => _output.WriteLine($"C# workflow final state: {DomainWorkflowRules.Render(state)}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Domain Workflow Comparison");
}
