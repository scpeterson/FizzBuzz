using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public static class IdempotentCommandRules
{
    public static Either<string, decimal> ParseAmount(string? number) =>
        decimal.TryParse(number, out var amount) && amount >= 0m
            ? Right<string, decimal>(amount)
            : Left<string, decimal>("Amount must be a non-negative decimal.");

    public static string NormalizeCommandId(string? commandId) =>
        string.IsNullOrWhiteSpace(commandId) ? "cmd-default" : commandId.Trim();

    public static (bool IsDuplicate, System.Collections.Generic.HashSet<string> Updated) HandleCSharp(IReadOnlySet<string> processed, string commandId)
    {
        var updated = new System.Collections.Generic.HashSet<string>(processed, StringComparer.OrdinalIgnoreCase);
        if (updated.Contains(commandId))
        {
            return (true, updated);
        }

        updated.Add(commandId);
        return (false, updated);
    }

    public static Either<string, System.Collections.Generic.HashSet<string>> HandleLanguageExt(System.Collections.Generic.HashSet<string> processed, string commandId) =>
        processed.Contains(commandId)
            ? Left<string, System.Collections.Generic.HashSet<string>>($"Duplicate command '{commandId}'.")
            : Right<string, System.Collections.Generic.HashSet<string>>(new System.Collections.Generic.HashSet<string>(processed, StringComparer.OrdinalIgnoreCase) { commandId });
}
