using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.TuplesExample;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class TupleDemo : IDemo
{
    public const string DemoKey = "tuple-demo";

    public string Key => DemoKey;
    public string Category => "csharp-support";
    public IReadOnlyCollection<string> Tags => ["csharp", "supporting-feature", "tuples", "baseline"];
    public string Description => "Supporting C# feature demo: tuple syntax and multiple-return patterns that show up in later functional examples.";
    public DemoExecutionResult Run(string? name, string? number)
    {
        // This demo is kept as supporting groundwork for later FP-oriented demos,
        // especially where tuples make intermediate values or multiple returns lighter-weight.
        ExecuteWithSpacing(OldTuple, nameof(OldTuple));
        ExecuteWithSpacing(NewTuple, nameof(NewTuple));
        ExecuteWithSpacing(NamedTuple, nameof(NamedTuple));
        ExecuteWithSpacing(ShowMultipleReturnTuple, nameof(ShowMultipleReturnTuple));
        ExecuteWithSpacing(ShowTupleWithLinq, nameof(ShowTupleWithLinq));
        return DemoExecutionResult.Success();
    }
}
