using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public class CSharpSeqMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpSeqMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpSeqMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-seq-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "seq", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from numbers in SeqMonadRules.ResolveNumbers(name)
                from threshold in SeqMonadRules.ParseThreshold(number)
                select numbers.Where(n => n >= threshold).Select(n => n * n).Sum();

            result.Match(
                Right: sum => _output.WriteLine($"Sum: {sum}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Seq Monad Comparison");
}
