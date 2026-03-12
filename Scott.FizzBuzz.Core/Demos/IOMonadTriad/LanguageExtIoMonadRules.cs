using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public static class LanguageExtIoMonadRules
{
    public static Either<string, decimal> ParseWeight(string? input) =>
        IoMonadRules.TryParseWeight(input, out var weight, out var error)
            ? Right<string, decimal>(weight)
            : Left<string, decimal>(error ?? "Weight must be numeric between 1 and 200.");

    public static Either<string, IoRuntimeProfile> ResolveProfile(string? name) =>
        IoMonadRules.TryResolveProfile(name, out var profile, out var error)
            ? Right<string, IoRuntimeProfile>(profile!)
            : Left<string, IoRuntimeProfile>(error ?? "Unknown profile. Use standard, priority, or economy.");
}
