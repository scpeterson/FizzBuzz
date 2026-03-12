using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;

public class ImperativeValidationMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeValidationMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeValidationMonadComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-validation-monad-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "validation", "monad"];
    public string Description => "Manual first-error validation flow with nested branching and early returns.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var checks = 0;

            checks++;
            if (string.IsNullOrWhiteSpace(name))
            {
                _output.WriteLine("Failed: Name is required.");
                _output.WriteLine($"Imperative checks executed: {checks}");
                return;
            }

            var normalizedName = name.Trim();
            checks++;
            if (normalizedName.Length < 3)
            {
                _output.WriteLine("Failed: Name must be at least 3 characters.");
                _output.WriteLine($"Imperative checks executed: {checks}");
                return;
            }

            checks++;
            if (normalizedName.Any(ch => !char.IsLetter(ch)))
            {
                _output.WriteLine("Failed: Name must contain letters only.");
                _output.WriteLine($"Imperative checks executed: {checks}");
                return;
            }

            checks++;
            if (!int.TryParse(number, out var age))
            {
                _output.WriteLine("Failed: Age must be numeric.");
                _output.WriteLine($"Imperative checks executed: {checks}");
                return;
            }

            checks++;
            if (age < 18 || age > 120)
            {
                _output.WriteLine("Failed: Age must be between 18 and 120.");
                _output.WriteLine($"Imperative checks executed: {checks}");
                return;
            }

            _output.WriteLine($"Result: validated candidate = {normalizedName} ({age})");
            _output.WriteLine($"Imperative checks executed: {checks}");
        }, "Imperative Validation Monad Comparison");
}
