namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public static class WriterMonadRules
{
    public static bool TryResolveOps(string? name, out IReadOnlyList<int>? ops, out string? error)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
        switch (key)
        {
            case "standard":
                ops = [2, 3, -1];
                error = null;
                return true;
            case "aggressive":
                ops = [3, 4, -2, 5];
                error = null;
                return true;
            case "scott":
                ops = [2, 2, -1];
                error = null;
                return true;
            default:
                ops = null;
                error = "Unknown writer profile. Use standard or aggressive.";
                return false;
        }
    }

    public static bool TryParseStart(string? number, out int start, out string? error)
    {
        if (int.TryParse(number, out start))
        {
            error = null;
            return true;
        }

        error = "Start value must be numeric.";
        return false;
    }

    public static (int NextState, string LogEntry) Step(int state, int op)
    {
        var next = op >= 0 ? state + op : state - Math.Abs(op);
        var msg = op >= 0
            ? $"Added {op}, state={next}"
            : $"Subtracted {Math.Abs(op)}, state={next}";

        return (next, msg);
    }
}
