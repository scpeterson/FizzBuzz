using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public static class SeqMonadRules
{
    public static Either<string, Seq<int>> ResolveNumbers(string? name)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
        return key switch
        {
            "standard" => Right<string, Seq<int>>(Seq(1, 2, 3, 4, 5, 6)),
            "large" => Right<string, Seq<int>>(Seq(10, 20, 30, 40)),
            "scott" => Right<string, Seq<int>>(Seq(2, 4, 6, 8)),
            _ => Left<string, Seq<int>>("Unknown sequence profile. Use standard or large.")
        };
    }

    public static Either<string, int> ParseThreshold(string? number) =>
        int.TryParse(number, out var threshold)
            ? Right<string, int>(threshold)
            : Left<string, int>("Threshold must be numeric.");

    public static int Compute(Seq<int> numbers, int threshold) =>
        numbers
            .Filter(n => n >= threshold)
            .Map(n => n * n)
            .Fold(0, (sum, value) => sum + value);
}
