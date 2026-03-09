using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.AffMonadTriad;

public class ImperativeAffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeAffMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeAffMonadComparisonDemo(IOutput output) => _output = output;

    public string Key => "imperative-aff-monad-comparison";
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "aff", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!int.TryParse(number, out var count) || count < 1 || count > 100)
            {
                _output.WriteLine("Failed: Count must be between 1 and 100.");
                return;
            }

            var key = string.IsNullOrWhiteSpace(name) ? "fast" : name.Trim().ToLowerInvariant();
            var delay = key switch
            {
                "fast" => 5,
                "slow" => 15,
                "scott" => 8,
                _ => -1
            };

            if (delay < 0)
            {
                _output.WriteLine("Failed: Unknown mode. Use fast or slow.");
                return;
            }

            Task.Delay(delay).GetAwaiter().GetResult();
            _output.WriteLine($"Result: {count * 2}");
        }, "Imperative Aff Monad Comparison");
}
