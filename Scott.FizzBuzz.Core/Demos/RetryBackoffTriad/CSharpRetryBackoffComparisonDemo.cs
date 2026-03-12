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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ResolvePolicy(name)
                .Bind(policy => ParseFailures(number).Map(failuresBeforeSuccess => (policy, failuresBeforeSuccess)))
                .Map(args => (PolicyName: args.policy.Name, Execution: RetryBackoffRules.ExecuteCSharpPipeline(args.policy, args.failuresBeforeSuccess)));

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: {RetryBackoffRules.FormatSummary(result.Value.Execution)}");
                _output.WriteLine($"Policy: {result.Value.PolicyName}");
                _output.WriteLine($"Backoff schedule: {RetryBackoffRules.FormatSchedule(result.Value.Execution.BackoffSchedule)}");
                return;
            }

            _output.WriteLine($"Failed: {result.Error}");
        }, "C# Retry + Backoff Comparison");

    private static DemoResult<RetryBackoffRules.RetryPolicy> ResolvePolicy(string? name) =>
        RetryBackoffRules.TryResolvePolicy(name, out var policy, out var error)
            ? DemoResult<RetryBackoffRules.RetryPolicy>.Success(policy!)
            : DemoResult<RetryBackoffRules.RetryPolicy>.Failure(error);

    private static DemoResult<int> ParseFailures(string? number) =>
        RetryBackoffRules.TryParseFailuresBeforeSuccess(number, out var failures, out var error)
            ? DemoResult<int>.Success(failures)
            : DemoResult<int>.Failure(error);

    private readonly record struct DemoResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DemoResult<T> Success(T value) => new(true, value, null);
        public static DemoResult<T> Failure(string? error) => new(false, default!, error);

        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess ? next(Value) : DemoResult<TNext>.Failure(Error);

        public DemoResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess ? DemoResult<TNext>.Success(map(Value)) : DemoResult<TNext>.Failure(Error);
    }
}
