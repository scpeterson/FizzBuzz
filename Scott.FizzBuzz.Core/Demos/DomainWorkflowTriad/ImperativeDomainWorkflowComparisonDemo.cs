using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!decimal.TryParse(number, out var amount) || amount < 0m)
            {
                _output.WriteLine("Failed: Amount must be a non-negative decimal.");
                return;
            }

            var env = new InMemoryFunctionalDemoEnvironment();
            var status = "Draft";

            if (amount > env.MaxAutoApproveAmount)
            {
                _output.WriteLine($"Failed: Amount {amount:0.00} exceeds auto-approval limit {env.MaxAutoApproveAmount:0.00}.");
                return;
            }

            status = "Authorized";
            status = "Packed";
            status = "Shipped";

            _output.WriteLine($"Imperative workflow final state: {status}");
        }, "Imperative Domain Workflow Comparison");
}
