using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public static class LanguageExtTryMonadRules
{
    public static Either<string, decimal> ParseInput(string? value) =>
        TryMonadRules.TryParseInput(value, out var parsed, out var error)
            ? Right<string, decimal>(parsed)
            : Left<string, decimal>(error ?? "Input must be numeric.");

    public static Try<decimal> InverseTry(decimal value) => Try(() => TryMonadRules.RiskyInverse(value));
}
