using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public static class LanguageExtIdempotentCommandRules
{
    public static Either<string, decimal> ParseAmount(string? number) =>
        IdempotentCommandRules.TryParseAmount(number, out var amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Amount must be a non-negative decimal.");

    public static Either<string, System.Collections.Generic.HashSet<string>> HandleLanguageExt(System.Collections.Generic.HashSet<string> processed, string commandId) =>
        processed.Contains(commandId)
            ? Left<string, System.Collections.Generic.HashSet<string>>($"Duplicate command '{commandId}'.")
            : Right<string, System.Collections.Generic.HashSet<string>>(new System.Collections.Generic.HashSet<string>(processed, StringComparer.OrdinalIgnoreCase) { commandId });
}
