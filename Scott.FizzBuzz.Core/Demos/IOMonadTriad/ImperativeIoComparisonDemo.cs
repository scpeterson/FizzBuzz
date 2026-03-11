using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public class ImperativeIoComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeIoComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeIoComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-io-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "io", "monad"];
    public string Description => "Imperative side-effect flow with explicit sequencing and manual error branching.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var profileEither = IoMonadRules.ResolveProfile(name);
            if (profileEither.IsLeft)
            {
                _output.WriteLine($"Failed: {profileEither.LeftToList()[0]}");
                return;
            }

            var weightEither = IoMonadRules.ParseWeight(number);
            if (weightEither.IsLeft)
            {
                _output.WriteLine($"Failed: {weightEither.LeftToList()[0]}");
                return;
            }

            var profile = profileEither.RightToList()[0];
            var weight = weightEither.RightToList()[0];
            var quote = IoMonadRules.CalculateQuote(weight, profile);
            var audit = IoMonadRules.FormatAudit(profile, weight, quote);

            _output.WriteLine($"Quote: {quote:0.00}");
            _output.WriteLine($"Audit: {audit}");
            _output.WriteLine("Imperative comparison note: side-effect ordering is manual and interleaved with branching.");
        }, "Imperative IO Comparison");
}
