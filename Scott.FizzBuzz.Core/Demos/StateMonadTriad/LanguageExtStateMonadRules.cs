using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StateMonadTriad;

public static class LanguageExtStateMonadRules
{
    public static Either<string, int> ParseStep(string? input) =>
        StateMonadRules.TryParseStep(input, out var step, out var error)
            ? Right<string, int>(step)
            : Left<string, int>(error ?? "Step must be a number between 1 and 100.");

    public static Either<string, Seq<string>> ResolvePlan(string? name) =>
        StateMonadRules.TryResolvePlan(name, out var plan, out var error)
            ? Right<string, Seq<string>>(toSeq(plan!))
            : Left<string, Seq<string>>(error ?? "Unknown plan. Use standard, aggressive, or defensive.");
}
