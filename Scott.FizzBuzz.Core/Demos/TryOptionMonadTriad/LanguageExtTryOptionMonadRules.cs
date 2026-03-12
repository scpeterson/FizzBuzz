using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public static class LanguageExtTryOptionMonadRules
{
    public static Either<string, int> ParseId(string? number) =>
        TryOptionMonadRules.TryParseId(number, out var id, out var error)
            ? Right<string, int>(id)
            : Left<string, int>(error ?? "Id must be numeric.");

    public static Option<decimal> LookupOption(int id)
    {
        var value = TryOptionMonadRules.LookupNullable(id);
        return value.HasValue ? Some(value.Value) : None;
    }

    public static TryOption<decimal> LookupTryOption(int id) =>
        TryOption(() => LookupOption(id));
}
