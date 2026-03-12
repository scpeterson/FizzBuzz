using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;

public class ImperativeDomainWorkflowComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeDomainWorkflowComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeDomainWorkflowComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-domain-workflow";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "domain-modeling", "triad"];
    public string Description => "Imperative domain workflow with explicit mutable progression through each fulfillment step.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!DomainWorkflowRules.TryParseAmount(number, out var amount, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var env = new InMemoryFunctionalDemoEnvironment();
            var draft = DomainWorkflowRules.CreateDraft(amount);

            if (!DomainWorkflowRules.TryAuthorize(env, draft, out var authorized, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var packed = DomainWorkflowRules.Pack(authorized!);
            var shipped = DomainWorkflowRules.Ship(packed);

            _output.WriteLine($"Result: {DomainWorkflowRules.Render(shipped)}");
        }, "Imperative Domain Workflow Comparison");
}
