using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EffMonadTriad;

public static class EffMonadRules
{
    public static Either<string, int> ParseUnits(string? number) =>
        int.TryParse(number, out var units) && units is >= 1 and <= 100
            ? Right<string, int>(units)
            : Left<string, int>("Units must be between 1 and 100.");

    public static Either<string, decimal> ResolveRate(string? name)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
        return key switch
        {
            "standard" => Right<string, decimal>(1.25m),
            "priority" => Right<string, decimal>(2.10m),
            "scott" => Right<string, decimal>(1.50m),
            _ => Left<string, decimal>("Unknown rate profile. Use standard or priority.")
        };
    }

    public static Eff<Either<string, decimal>> ComputeEff(string? name, string? number) =>
        Eff(() =>
            from units in ParseUnits(number)
            from rate in ResolveRate(name)
            select units * rate);
}
