using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.TuplesExample;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class TupleDemo : IDemo
{
    public const string DemoKey = "tuple-demo";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "tuples", "baseline"];
    public Either<string, Unit> Run(string? name, string? number)
    {
        ExecuteWithSpacing(OldTuple, nameof(OldTuple));
        ExecuteWithSpacing(NewTuple, nameof(NewTuple));
        ExecuteWithSpacing(NamedTuple, nameof(NamedTuple));
        ExecuteWithSpacing(ShowMultipleReturnTuple, nameof(ShowMultipleReturnTuple));
        ExecuteWithSpacing(ShowTupleWithLinq, nameof(ShowTupleWithLinq));
        return unit;
    }
}
