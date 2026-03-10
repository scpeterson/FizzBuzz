using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.PatternMatchingExample;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class PatternMatchingDemo : IDemo
{
    public string Key => "pattern-matching";
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "pattern-matching", "baseline"];
    public Either<string, Unit> Run(string? name, string? number)
    {
        ExecuteWithSpacing(SimpleTypePattern, nameof(SimpleTypePattern));
        ExecuteWithSpacing(PropertyPattern, nameof(PropertyPattern));
        ExecuteWithSpacing(TuplePatterns, nameof(TuplePatterns));
        ExecuteWithSpacing(() => RelationalPattern(5), nameof(RelationalPattern));
        ExecuteWithSpacing(() => LogicalPatterns('A'), nameof(LogicalPatterns));
        ExecuteWithSpacing(PositionalPatterns, nameof(PositionalPatterns));
        ExecuteWithSpacing(SwitchExpression, nameof(SwitchExpression));
        ExecuteWithSpacing(PatternCombinators, nameof(PatternCombinators));
        return unit;
    }
}
