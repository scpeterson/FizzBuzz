using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;

public class LanguageExtRetryBackoffComparisonDemo : IDemo
{
    public const string DemoKey = "langext-retry-backoff-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "retry", "backoff", "policy"];
    public string Description => "Pure retry/backoff policy encoded as deterministic transformations with no wait or I/O effects.";

    public Either<string, Unit> Run(string? name, string? number) =>
        RetryBackoffRules.ResolvePolicy(name)
            .Bind(policy =>
                RetryBackoffRules.ParseFailuresBeforeSuccess(number)
                    .Map(failuresBeforeSuccess => RetryBackoffRules.ExecuteLanguageExtPipeline(policy, failuresBeforeSuccess)))
            .Bind(execution =>
                execution.Success
                    ? Right<string, RetryBackoffRules.RetryExecutionResult>(execution)
                    : Left<string, RetryBackoffRules.RetryExecutionResult>("Operation still failed after exhausting retries."))
            .Map(_ => unit);
}
