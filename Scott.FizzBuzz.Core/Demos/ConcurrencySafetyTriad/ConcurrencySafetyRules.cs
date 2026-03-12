using System.Threading;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public static class ConcurrencySafetyRules
{
    public sealed record ConcurrencySimulationResult(int InitialBalance, int ExpectedBalance, int FinalBalance)
    {
        public int LostUpdates => Math.Max(0, ExpectedBalance - FinalBalance);
    }

    public static bool TryParseIterations(string? value, out int iterations, out string? error)
    {
        if (!int.TryParse(value, out iterations))
        {
            error = "Iterations must be an integer.";
            return false;
        }

        if (iterations is < 1 or > 1_000_000)
        {
            error = "Iterations must be between 1 and 1000000.";
            return false;
        }

        error = null;
        return true;
    }

    public static ConcurrencySimulationResult ExecuteImperativeUnsafe(int iterations)
    {
        var balance = 0;

        for (var i = 0; i < iterations; i++)
        {
            var workerARead = balance;
            var workerBRead = balance;

            balance = workerARead + 1;
            balance = workerBRead + 1;
        }

        return new ConcurrencySimulationResult(0, iterations * 2, balance);
    }

    public static ConcurrencySimulationResult ExecuteCSharpAtomic(int iterations)
    {
        var balance = 0;

        for (var i = 0; i < iterations * 2; i++)
        {
            Interlocked.Increment(ref balance);
        }

        return new ConcurrencySimulationResult(0, iterations * 2, balance);
    }

    public static string FormatSummary(ConcurrencySimulationResult result) =>
        $"Expected={result.ExpectedBalance}, Final={result.FinalBalance}, LostUpdates={result.LostUpdates}";
}
