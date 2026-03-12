namespace Scott.FizzBuzz.Core.Demos.StateMonadTriad;

public static class StateMonadRules
{
    private static readonly IReadOnlyDictionary<string, string[]> Plans =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["standard"] = ["add", "boost", "add"],
            ["aggressive"] = ["boost", "add", "add", "penalty", "add"],
            ["defensive"] = ["add", "penalty", "add"],
            ["scott"] = ["add", "boost", "add"]
        };

    public static bool TryParseStep(string? input, out int step, out string? error)
    {
        if (int.TryParse(input, out step) && step is >= 1 and <= 100)
        {
            error = null;
            return true;
        }

        error = "Step must be a number between 1 and 100.";
        return false;
    }

    public static bool TryResolvePlan(string? name, out IReadOnlyList<string>? plan, out string? error)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim();
        if (Plans.TryGetValue(key, out var resolved))
        {
            plan = resolved;
            error = null;
            return true;
        }

        plan = null;
        error = "Unknown plan. Use standard, aggressive, or defensive.";
        return false;
    }

    public static StateGame Apply(string operation, int step, StateGame state) =>
        operation switch
        {
            "add" => state with { Score = state.Score + (step * state.Multiplier) },
            "boost" => state with { Multiplier = state.Multiplier + 1 },
            "penalty" => state with { Score = state.Score - 3, Penalties = state.Penalties + 1 },
            _ => state
        };
}
