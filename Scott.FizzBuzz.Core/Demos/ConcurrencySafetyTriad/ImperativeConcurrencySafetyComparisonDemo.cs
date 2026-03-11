using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public class ImperativeConcurrencySafetyComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeConcurrencySafetyComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeConcurrencySafetyComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-concurrency-safety-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "concurrency", "safety"];
    public string Description => "Unsafe mutable read-modify-write flow that demonstrates lost updates under concurrent interleaving.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            ConcurrencySafetyRules.ParseIterations(number).Match(
                Right: iterations =>
                {
                    var result = ConcurrencySafetyRules.ExecuteImperativeUnsafe(iterations);
                    _output.WriteLine(ConcurrencySafetyRules.FormatSummary(result));
                    _output.WriteLine("Imperative note: non-atomic read-modify-write can lose updates.");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "Imperative Concurrency Safety Comparison");
}
