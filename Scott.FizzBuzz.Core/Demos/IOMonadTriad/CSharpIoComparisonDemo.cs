using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public class CSharpIoComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpIoComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpIoComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-io-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "io", "monad"];
    public string Description => "C# functional composition with explicit side-effect boundaries and manual orchestration.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from profile in IoMonadRules.ResolveProfile(name)
                from weight in IoMonadRules.ParseWeight(number)
                select (profile, weight, quote: IoMonadRules.CalculateQuote(weight, profile));

            result.Match(
                Right: tuple =>
                {
                    var audit = IoMonadRules.FormatAudit(tuple.profile, tuple.weight, tuple.quote);
                    _output.WriteLine($"Quote: {tuple.quote:0.00}");
                    _output.WriteLine($"Audit: {audit}");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));

            _output.WriteLine("C#/.NET comparison note: side effects are explicit but still manually sequenced.");
        }, "C# IO Comparison");
}
