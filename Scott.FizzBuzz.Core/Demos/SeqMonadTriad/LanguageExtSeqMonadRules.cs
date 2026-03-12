using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public static class LanguageExtSeqMonadRules
{
    public static Either<string, Seq<int>> ResolveNumbers(string? name) =>
        SeqMonadRules.TryResolveNumbers(name, out var numbers, out var error)
            ? Right<string, Seq<int>>(toSeq(numbers!))
            : Left<string, Seq<int>>(error ?? "Unknown sequence profile. Use standard or large.");

    public static Either<string, int> ParseThreshold(string? number) =>
        SeqMonadRules.TryParseThreshold(number, out var threshold, out var error)
            ? Right<string, int>(threshold)
            : Left<string, int>(error ?? "Threshold must be numeric.");
}
