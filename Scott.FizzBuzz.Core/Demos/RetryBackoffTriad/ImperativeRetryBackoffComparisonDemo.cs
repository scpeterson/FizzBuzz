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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!RetryBackoffRules.TryResolvePolicy(name, out var policy, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!RetryBackoffRules.TryParseFailuresBeforeSuccess(number, out var failuresBeforeSuccess, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var result = RetryBackoffRules.ExecuteImperative(policy!, failuresBeforeSuccess);
            _output.WriteLine($"Result: {RetryBackoffRules.FormatSummary(result)}");
            _output.WriteLine($"Policy: {policy!.Name}");
            _output.WriteLine($"Backoff schedule: {RetryBackoffRules.FormatSchedule(result.BackoffSchedule)}");
        }, "Imperative Retry + Backoff Comparison");
}
