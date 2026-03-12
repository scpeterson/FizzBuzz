using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public static class LanguageExtConcurrencySafetyRules
{
    public static Either<string, int> ParseIterations(string? value) =>
        ConcurrencySafetyRules.TryParseIterations(value, out var iterations, out var error)
            ? Right<string, int>(iterations)
            : Left<string, int>(error ?? "Iterations must be an integer.");

    public static ConcurrencySafetyRules.ConcurrencySimulationResult ExecuteLanguageExtPure(int iterations)
    {
        var finalBalance = Range(1, iterations * 2).Fold(0, (state, _) => state + 1);
        return new ConcurrencySafetyRules.ConcurrencySimulationResult(0, iterations * 2, finalBalance);
    }
}
