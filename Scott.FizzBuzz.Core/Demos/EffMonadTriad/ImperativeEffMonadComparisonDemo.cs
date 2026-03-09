using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EffMonadTriad;

public class ImperativeEffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeEffMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeEffMonadComparisonDemo(IOutput output) => _output = output;

    public string Key => "imperative-eff-monad-comparison";
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "eff", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!int.TryParse(number, out var units) || units < 1 || units > 100)
            {
                _output.WriteLine("Failed: Units must be between 1 and 100.");
                return;
            }

            var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
            decimal rate;
            if (key == "standard") rate = 1.25m;
            else if (key == "priority") rate = 2.10m;
            else if (key == "scott") rate = 1.50m;
            else
            {
                _output.WriteLine("Failed: Unknown rate profile. Use standard or priority.");
                return;
            }

            _output.WriteLine($"Total: {(units * rate):0.00}");
        }, "Imperative Eff Monad Comparison");
}
