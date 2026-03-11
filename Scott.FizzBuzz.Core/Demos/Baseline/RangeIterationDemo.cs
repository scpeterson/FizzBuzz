using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.ImperativeExample;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class RangeIterationDemo : IDemo
{
    private readonly IOutput _output;

    public RangeIterationDemo() : this(new ConsoleOutput())
    {
    }

    public RangeIterationDemo(IOutput output)
    {
        _output = output;
    }

    // This companion demo exists to contrast with ImperativeDemo:
    // same fizzbuzz logic, but expressed as a functional data pipeline.
    public const string DemoKey = "range-iter";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "comparison", "seq", "baseline"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, ShowRangeIterationFizzBuzz, nameof(ShowRangeIterationFizzBuzz));

    private void ShowRangeIterationFizzBuzz()
    {
        // Sequence pipeline: transform each number then iterate for output.
        Range(1, 100)
            .Map(ImperativeFizzBuzz)
            .Iter(_output.WriteLine);
    }
}
