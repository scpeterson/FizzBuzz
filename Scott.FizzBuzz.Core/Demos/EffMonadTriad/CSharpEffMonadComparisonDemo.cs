using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EffMonadTriad;

public class CSharpEffMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpEffMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpEffMonadComparisonDemo(IOutput output) => _output = output;

    public string Key => "csharp-eff-monad-comparison";
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "eff", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from units in EffMonadRules.ParseUnits(number)
                from rate in EffMonadRules.ResolveRate(name)
                select units * rate;

            result.Match(
                Right: total => _output.WriteLine($"Total: {total:0.00}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Eff Monad Comparison");
}
