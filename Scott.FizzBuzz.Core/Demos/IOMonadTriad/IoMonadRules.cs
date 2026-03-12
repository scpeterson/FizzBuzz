namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public static class IoMonadRules
{
    private static readonly IReadOnlyDictionary<string, IoRuntimeProfile> Profiles =
        new Dictionary<string, IoRuntimeProfile>(StringComparer.OrdinalIgnoreCase)
        {
            ["standard"] = new("Standard", 4.00m, 1.50m, 1.00m),
            ["priority"] = new("Priority", 6.00m, 2.25m, 1.35m),
            ["economy"] = new("Economy", 2.25m, 1.00m, 0.85m),
            ["scott"] = new("Scott", 3.00m, 1.10m, 1.00m)
        };

    public static bool TryParseWeight(string? input, out decimal weight, out string? error)
    {
        if (decimal.TryParse(input, out weight) && weight is >= 1m and <= 200m)
        {
            error = null;
            return true;
        }

        error = "Weight must be numeric between 1 and 200.";
        return false;
    }

    public static bool TryResolveProfile(string? name, out IoRuntimeProfile? profile, out string? error)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim();
        if (Profiles.TryGetValue(key, out profile))
        {
            error = null;
            return true;
        }

        error = "Unknown profile. Use standard, priority, or economy.";
        return false;
    }

    public static decimal CalculateQuote(decimal weight, IoRuntimeProfile profile) =>
        ((weight * profile.BaseRate) + profile.FuelSurcharge) * profile.Multiplier;

    public static string FormatAudit(IoRuntimeProfile profile, decimal weight, decimal quote) =>
        $"{profile.Name} quote computed for {weight:0.##}kg => {quote:0.00}";
}
