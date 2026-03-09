using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.AffMonadTriad;

public static class AffMonadRules
{
    public static Either<string, int> ParseCount(string? number) =>
        int.TryParse(number, out var count) && count is >= 1 and <= 100
            ? Right<string, int>(count)
            : Left<string, int>("Count must be between 1 and 100.");

    public static Either<string, int> ResolveDelayMs(string? name)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "fast" : name.Trim().ToLowerInvariant();
        return key switch
        {
            "fast" => Right<string, int>(5),
            "slow" => Right<string, int>(15),
            "scott" => Right<string, int>(8),
            _ => Left<string, int>("Unknown mode. Use fast or slow.")
        };
    }

    public static Aff<Either<string, int>> ComputeAff(string? name, string? number) =>
        Aff<Either<string, int>>(async () =>
        {
            var resolved =
                from count in ParseCount(number)
                from delay in ResolveDelayMs(name)
                select (count, delay);

            return await resolved.Match(
                Right: async tuple =>
                {
                    await Task.Delay(tuple.delay);
                    return Right<string, int>(tuple.count * 2);
                },
                Left: error => Task.FromResult(Left<string, int>(error)));
        });
}
