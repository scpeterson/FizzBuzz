namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public static class SeqMonadRules
{
    public static bool TryResolveNumbers(string? name, out IReadOnlyList<int>? numbers, out string? error)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
        switch (key)
        {
            case "standard":
                numbers = [1, 2, 3, 4, 5, 6];
                error = null;
                return true;
            case "large":
                numbers = [10, 20, 30, 40];
                error = null;
                return true;
            case "scott":
                numbers = [2, 4, 6, 8];
                error = null;
                return true;
            default:
                numbers = null;
                error = "Unknown sequence profile. Use standard or large.";
                return false;
        }
    }

    public static bool TryParseThreshold(string? number, out int threshold, out string? error)
    {
        if (int.TryParse(number, out threshold))
        {
            error = null;
            return true;
        }

        error = "Threshold must be numeric.";
        return false;
    }

    public static int Compute(IEnumerable<int> numbers, int threshold) =>
        numbers
            .Where(n => n >= threshold)
            .Select(n => n * n)
            .Sum();
}
