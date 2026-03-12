using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.NullOptionTriad;

public class ImperativeNullHandlingDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeNullHandlingDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeNullHandlingDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-null-handling";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "null"];
    public string Description => "Imperative null-handling with step-by-step checks and early returns.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var rawName = name;
            if (rawName == null)
            {
                _output.WriteLine("Name missing.");
                return;
            }

            var trimmed = rawName.Trim();
            if (trimmed.Length == 0)
            {
                _output.WriteLine("Name empty.");
                return;
            }

            _output.WriteLine($"Hello, {trimmed}.");
        }, "Imperative Null Handling");
}
