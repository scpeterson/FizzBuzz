using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;

public static class LanguageExtRetryBackoffRules
{
    private sealed record FoldState(bool Completed, bool Success, int Attempts, Seq<TimeSpan> Schedule);

    public static Either<string, RetryBackoffRules.RetryPolicy> ResolvePolicy(string? name) =>
        RetryBackoffRules.TryResolvePolicy(name, out var policy, out var error)
            ? Right<string, RetryBackoffRules.RetryPolicy>(policy!)
            : Left<string, RetryBackoffRules.RetryPolicy>(error ?? "Policy must be one of: exponential|exp|linear.");

    public static Either<string, int> ParseFailuresBeforeSuccess(string? number) =>
        RetryBackoffRules.TryParseFailuresBeforeSuccess(number, out var failures, out var error)
            ? Right<string, int>(failures)
            : Left<string, int>(error ?? "Failures-before-success must be an integer.");

    public static RetryBackoffRules.RetryExecutionResult ExecuteLanguageExtPipeline(
        RetryBackoffRules.RetryPolicy policy,
        int failuresBeforeSuccess)
    {
        var initial = new FoldState(Completed: false, Success: false, Attempts: 0, Schedule: Seq<TimeSpan>());

        var final = Range(1, policy.MaxRetries + 1)
            .Fold(initial, (state, attempt) =>
            {
                if (state.Completed)
                {
                    return state;
                }

                if (attempt > failuresBeforeSuccess)
                {
                    return state with { Completed = true, Success = true, Attempts = attempt };
                }

                var nextSchedule = attempt <= policy.MaxRetries
                    ? state.Schedule.Add(RetryBackoffRules.DelayForAttempt(policy, attempt))
                    : state.Schedule;

                var exhausted = attempt == policy.MaxRetries + 1;
                return state with
                {
                    Completed = exhausted,
                    Success = false,
                    Attempts = attempt,
                    Schedule = nextSchedule
                };
            });

        return new RetryBackoffRules.RetryExecutionResult(final.Success, final.Attempts, final.Schedule.ToArray());
    }
}
