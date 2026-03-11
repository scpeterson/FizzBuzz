using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;

public class CSharpRetryBackoffComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpRetryBackoffComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpRetryBackoffComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-retry-backoff-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "retry", "backoff", "policy"];
    public string Description => "Composed retry policy and schedule computation without sleeping, but still manually threaded through helpers.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = RetryBackoffRules.ResolvePolicy(name)
                .Bind(policy =>
                    RetryBackoffRules.ParseFailuresBeforeSuccess(number)
                        .Map(failuresBeforeSuccess => (policy, failuresBeforeSuccess)))
                .Map(args =>
                {
                    var execution = RetryBackoffRules.ExecuteCSharpPipeline(args.policy, args.failuresBeforeSuccess);
                    return (PolicyName: args.policy.Name, Execution: execution);
                });

            result.Match(
                Right: success =>
                {
                    _output.WriteLine($"Policy: {success.PolicyName}");
                    _output.WriteLine(RetryBackoffRules.FormatSummary(success.Execution));
                    _output.WriteLine($"Backoff schedule: {RetryBackoffRules.FormatSchedule(success.Execution.BackoffSchedule)}");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Retry + Backoff Comparison");
}
