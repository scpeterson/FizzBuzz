namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public static class IdempotentCommandRules
{
    public static bool TryParseAmount(string? number, out decimal amount, out string? error)
    {
        if (decimal.TryParse(number, out amount) && amount >= 0m)
        {
            error = null;
            return true;
        }

        error = "Amount must be a non-negative decimal.";
        return false;
    }

    public static string NormalizeCommandId(string? commandId) =>
        string.IsNullOrWhiteSpace(commandId) ? "cmd-default" : commandId.Trim();

    public static (bool IsDuplicate, HashSet<string> Updated) HandleCSharp(IReadOnlySet<string> processed, string commandId)
    {
        var updated = new HashSet<string>(processed, StringComparer.OrdinalIgnoreCase);
        if (updated.Contains(commandId))
        {
            return (true, updated);
        }

        updated.Add(commandId);
        return (false, updated);
    }
}
