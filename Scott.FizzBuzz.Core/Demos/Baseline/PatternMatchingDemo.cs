using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.PatternMatchingExample;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class PatternMatchingDemo : IDemo
{
    public const string DemoKey = "pattern-matching";

    public string Key => DemoKey;
    public string Category => "csharp-support";
    public IReadOnlyCollection<string> Tags => ["csharp", "supporting-feature", "pattern-matching", "baseline"];
    public string Description => "Supporting C# feature demo: pattern matching syntax that later helps express functional-style branching more clearly.";
    public DemoExecutionResult Run(string? name, string? number)
    {
        // This demo is intentionally a language-feature tour, not a core FP comparison.
        // It supports later demos by familiarizing learners with the pattern tools used there.
        ExecuteWithSpacing(SimpleTypePattern, nameof(SimpleTypePattern));
        ExecuteWithSpacing(PropertyPattern, nameof(PropertyPattern));
        ExecuteWithSpacing(TuplePatterns, nameof(TuplePatterns));
        ExecuteWithSpacing(() => RelationalPattern(5), nameof(RelationalPattern));
        ExecuteWithSpacing(() => LogicalPatterns('A'), nameof(LogicalPatterns));
        ExecuteWithSpacing(PositionalPatterns, nameof(PositionalPatterns));
        ExecuteWithSpacing(SwitchExpression, nameof(SwitchExpression));
        ExecuteWithSpacing(PatternCombinators, nameof(PatternCombinators));
        return DemoExecutionResult.Success();
    }
}
