using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ExceptionBoundariesTriad;

public class ImperativeExceptionBoundariesDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeExceptionBoundariesDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeExceptionBoundariesDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-exception-boundaries";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "exceptions"];
    public string Description => "Imperative boundary that relies on exceptions and try/catch for control flow.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            try
            {
                var denominator = int.Parse(number ?? "2");
                var result = 100 / denominator;
                _output.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "Imperative Exception Boundaries");
}
