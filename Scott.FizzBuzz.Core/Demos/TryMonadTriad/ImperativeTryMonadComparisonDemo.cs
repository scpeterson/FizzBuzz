using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public class ImperativeTryMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeTryMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeTryMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "imperative-try-monad-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "try", "monad"];
    public string Description => "Imperative exception-prone flow that relies on try/catch around risky calculations.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!decimal.TryParse(number, out var value))
            {
                _output.WriteLine("Failed: Input must be numeric.");
                return;
            }

            try
            {
                var inverse = TryMonadRules.RiskyInverse(value);
                _output.WriteLine($"Result: inverse = {inverse:0.####}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "Imperative Try Monad Comparison");
}
