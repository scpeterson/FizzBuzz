using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;

public class ImperativeRetryBackoffComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeRetryBackoffComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeRetryBackoffComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-retry-backoff-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "retry", "backoff", "policy"];
    public string Description => "Classic loop-based retry orchestration with mutable counters and hand-managed backoff schedule.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var policyResult = RetryBackoffRules.ResolvePolicy(name);
            var failureCountResult = RetryBackoffRules.ParseFailuresBeforeSuccess(number);

            policyResult.Match(
                Right: policy =>
                    failureCountResult.Match(
                        Right: failuresBeforeSuccess =>
                        {
                            var result = RetryBackoffRules.ExecuteImperative(policy, failuresBeforeSuccess);
                            _output.WriteLine($"Policy: {policy.Name}");
                            _output.WriteLine(RetryBackoffRules.FormatSummary(result));
                            _output.WriteLine($"Backoff schedule: {RetryBackoffRules.FormatSchedule(result.BackoffSchedule)}");
                        },
                        Left: error => _output.WriteLine($"Failed: {error}")),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "Imperative Retry + Backoff Comparison");
}
