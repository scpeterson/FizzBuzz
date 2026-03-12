using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public class ImperativeTryOptionMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeTryOptionMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeTryOptionMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "imperative-tryoption-monad-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "tryoption", "monad"];
    public string Description => "Imperative lookup flow combining parse checks, null handling, and explicit not-found branching.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!int.TryParse(number, out var id))
            {
                _output.WriteLine("Failed: Id must be numeric.");
                return;
            }

            try
            {
                var value = TryOptionMonadRules.LookupNullable(id);
                if (value.HasValue)
                {
                    _output.WriteLine($"Result: {value.Value:0.##}");
                }
                else
                {
                    _output.WriteLine("No value for id.");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "Imperative TryOption Monad Comparison");
}
