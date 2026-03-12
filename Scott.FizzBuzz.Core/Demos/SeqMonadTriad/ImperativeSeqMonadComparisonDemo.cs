using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public class ImperativeSeqMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeSeqMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeSeqMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "imperative-seq-monad-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "seq", "monad"];
    public string Description => "Imperative sequence processing with temporary lists, loops, and manual filtering steps.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!SeqMonadRules.TryParseThreshold(number, out var threshold, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!SeqMonadRules.TryResolveNumbers(name, out var numbers, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var sum = 0;
            foreach (var n in numbers!)
            {
                if (n >= threshold)
                {
                    sum += n * n;
                }
            }

            _output.WriteLine($"Result: sum = {sum}");
        }, "Imperative Seq Monad Comparison");
}
