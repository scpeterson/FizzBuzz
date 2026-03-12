using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;

public class ImperativeReaderComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeReaderComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeReaderComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-reader-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "reader", "monad"];
    public string Description => "Manual dependency lookups and branch-heavy orchestration without Reader composition.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var checks = 0;
            var context = ReaderMonadSampleData.ResolveProfile(name);

            checks++;
            if (context is null)
            {
                _output.WriteLine("Failed: Unknown pricing profile. Use standard, vip, or intl.");
                _output.WriteLine($"Imperative checks: {checks}");
                return;
            }

            checks++;
            if (!decimal.TryParse(number, out var subtotal))
            {
                _output.WriteLine("Failed: Subtotal must be a valid decimal value.");
                _output.WriteLine($"Imperative checks: {checks}");
                return;
            }

            checks++;
            if (subtotal < 1m || subtotal > 10000m)
            {
                _output.WriteLine("Failed: Subtotal must be between 1 and 10000.");
                _output.WriteLine($"Imperative checks: {checks}");
                return;
            }

            var taxed = subtotal * (1m + context.TaxRate);
            var total = taxed + context.ServiceFee;

            _output.WriteLine($"Result: {context.ProfileName}: total = {total:0.00} {context.Currency}");
            _output.WriteLine($"Imperative checks: {checks}");
        }, "Imperative Reader Comparison");
}
