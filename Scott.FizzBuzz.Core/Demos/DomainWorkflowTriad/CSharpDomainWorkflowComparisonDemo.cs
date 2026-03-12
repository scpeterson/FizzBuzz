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
    public string Description => "Plain C# domain workflow with explicit result composition across immutable fulfillment states.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var env = new InMemoryFunctionalDemoEnvironment();
            var result = ParseAmount(number)
                .Bind(amount => Success<DomainWorkflowRules.Draft, string>(DomainWorkflowRules.CreateDraft(amount)))
                .Bind(draft => Authorize(env, draft))
                .Map(DomainWorkflowRules.Pack)
                .Map(DomainWorkflowRules.Ship);

            if (result.IsSuccess && result.Value is not null)
            {
                _output.WriteLine($"Result: {DomainWorkflowRules.Render(result.Value)}");
                return;
            }

            _output.WriteLine($"Failed: {result.Error}");
        }, "C# Domain Workflow Comparison");

    private static DemoResult<decimal> ParseAmount(string? number) =>
        DomainWorkflowRules.TryParseAmount(number, out var amount, out var error)
            ? Success<decimal, string>(amount)
            : Failure<decimal>(error ?? "Amount must be a non-negative decimal.");

    private static DemoResult<DomainWorkflowRules.Authorized> Authorize(
        InMemoryFunctionalDemoEnvironment env,
        DomainWorkflowRules.Draft draft) =>
        DomainWorkflowRules.TryAuthorize(env, draft, out var authorized, out var error)
            ? Success<DomainWorkflowRules.Authorized, string>(authorized!)
            : Failure<DomainWorkflowRules.Authorized>(error ?? "Authorization failed.");

    private static DemoResult<T> Success<T, TError>(T value) => new(value, null);

    private static DemoResult<T> Failure<T>(string error) => new(default, error);

    private readonly record struct DemoResult<T>(T? Value, string? Error)
    {
        public bool IsSuccess => Error is null;

        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess && Value is not null
                ? next(Value)
                : new DemoResult<TNext>(default, Error);

        public DemoResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess && Value is not null
                ? new DemoResult<TNext>(map(Value), null)
                : new DemoResult<TNext>(default, Error);
    }
}
