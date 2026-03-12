namespace Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;

public static class RetryBackoffRules
{
    public sealed record RetryPolicy(string Name, int MaxRetries, TimeSpan InitialDelay, decimal Multiplier);

    public sealed record RetryExecutionResult(bool Success, int Attempts, IReadOnlyList<TimeSpan> BackoffSchedule)
    {
        public int RetriesUsed => Math.Max(0, Attempts - 1);
    }

    private sealed record FoldState(bool Completed, bool Success, int Attempts, IReadOnlyList<TimeSpan> Schedule);

    public static bool TryResolvePolicy(string? name, out RetryPolicy? policy, out string? error)
    {
        switch ((name ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "linear":
                policy = new RetryPolicy("linear", MaxRetries: 5, InitialDelay: TimeSpan.FromMilliseconds(100), Multiplier: 1m);
                error = null;
                return true;
            case "exponential":
            case "exp":
            case "":
                policy = new RetryPolicy("exponential", MaxRetries: 5, InitialDelay: TimeSpan.FromMilliseconds(100), Multiplier: 2m);
                error = null;
                return true;
            default:
                policy = null;
                error = "Policy must be one of: exponential|exp|linear.";
                return false;
        }
    }

    public static bool TryParseFailuresBeforeSuccess(string? number, out int failuresBeforeSuccess, out string? error)
    {
        if (!int.TryParse(number, out failuresBeforeSuccess))
        {
            error = "Failures-before-success must be an integer.";
            return false;
        }

        if (failuresBeforeSuccess is < 0 or > 10)
        {
            error = "Failures-before-success must be between 0 and 10.";
            return false;
        }

        error = null;
        return true;
    }

    public static TimeSpan DelayForAttempt(RetryPolicy policy, int retryAttempt)
    {
        var multiplier = policy.Name == "linear"
            ? retryAttempt
            : Pow(policy.Multiplier, retryAttempt - 1);

        var milliseconds = (double)policy.InitialDelay.TotalMilliseconds * (double)multiplier;
        return TimeSpan.FromMilliseconds(milliseconds);
    }

    public static RetryExecutionResult ExecuteImperative(RetryPolicy policy, int failuresBeforeSuccess)
    {
        var attempt = 0;
        var schedule = new List<TimeSpan>();

        while (attempt <= policy.MaxRetries)
        {
            attempt++;
            var shouldFail = attempt <= failuresBeforeSuccess;

            if (!shouldFail)
            {
                return new RetryExecutionResult(Success: true, Attempts: attempt, BackoffSchedule: schedule);
            }

            if (attempt <= policy.MaxRetries)
            {
                schedule.Add(DelayForAttempt(policy, attempt));
            }
        }

        return new RetryExecutionResult(Success: false, Attempts: policy.MaxRetries + 1, BackoffSchedule: schedule);
    }

    public static RetryExecutionResult ExecuteCSharpPipeline(RetryPolicy policy, int failuresBeforeSuccess)
    {
        var initial = new FoldState(Completed: false, Success: false, Attempts: 0, Schedule: Array.Empty<TimeSpan>());

        var final = Enumerable.Range(1, policy.MaxRetries + 1)
            .Aggregate(initial, (state, attempt) =>
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
                    ? state.Schedule.Append(DelayForAttempt(policy, attempt)).ToArray()
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

        return new RetryExecutionResult(final.Success, final.Attempts, final.Schedule);
    }

    public static string FormatSchedule(IReadOnlyList<TimeSpan> schedule) =>
        schedule.Count == 0
            ? "[none]"
            : string.Join(", ", schedule.Select(delay => $"{delay.TotalMilliseconds:0}ms"));

    public static string FormatSummary(RetryExecutionResult result) =>
        result.Success
            ? $"Succeeded after {result.Attempts} attempt(s) with {result.RetriesUsed} retries."
            : $"Failed after {result.Attempts} attempt(s): retries exhausted.";

    private static decimal Pow(decimal baseValue, int exponent)
    {
        var value = 1m;
        for (var i = 0; i < exponent; i++)
        {
            value *= baseValue;
        }

        return value;
    }
}
