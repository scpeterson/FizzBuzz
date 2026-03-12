namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public static class TryOptionMonadRules
{
    private static readonly IReadOnlyDictionary<int, decimal> Values = new Dictionary<int, decimal>
    {
        [1] = 10.5m,
        [2] = 22.0m,
        [21] = 99.9m
    };

    public static bool TryParseId(string? number, out int id, out string? error)
    {
        if (int.TryParse(number, out id))
        {
            error = null;
            return true;
        }

        error = "Id must be numeric.";
        return false;
    }

    public static decimal? LookupNullable(int id)
    {
        if (id == 13)
        {
            throw new InvalidOperationException("Repository unavailable for id 13.");
        }

        return Values.TryGetValue(id, out var value) ? value : null;
    }
}
