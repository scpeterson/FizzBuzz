using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public class LanguageExtIdempotentCommandComparisonDemo : IDemo
{
    public string Key => "langext-idempotent-command";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "idempotency", "triad"];

    public Either<string, Unit> Run(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();
        var commandId = IdempotentCommandRules.NormalizeCommandId(name);
        var initial = new System.Collections.Generic.HashSet<string>(env.SeedProcessedCommandIds, StringComparer.OrdinalIgnoreCase);

        var result =
            from _ in IdempotentCommandRules.ParseAmount(number)
            from updated in IdempotentCommandRules.HandleLanguageExt(initial, commandId)
            select updated;

        return result.Map(_ => unit);
    }
}
