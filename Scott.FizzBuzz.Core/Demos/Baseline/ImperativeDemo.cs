using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.ImperativeExample;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class ImperativeDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeDemo(IOutput output)
    {
        _output = output;
    }

    // This demo intentionally uses the classic imperative style so learners can
    // compare it directly with the functional sequence-based demo.
    public const string DemoKey = "imperative";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "baseline"];
    public string Description => "Baseline imperative FizzBuzz using a mutable loop and per-iteration output side effects.";
    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, ShowImperativeFizzBuzz, nameof(ShowImperativeFizzBuzz));

    private void ShowImperativeFizzBuzz()
    {
        // Deliberately imperative: mutable loop counter + per-iteration side effects.
        for (var i = 1; i <= 100; i++)
        {
            var result = ImperativeFizzBuzz(i);
            _output.WriteLine(result);
        }
    }
}
