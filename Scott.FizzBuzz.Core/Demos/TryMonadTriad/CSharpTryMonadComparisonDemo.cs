using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public class CSharpTryMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpTryMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpTryMonadComparisonDemo(IOutput output) => _output = output;

    public string Key => "csharp-try-monad-comparison";
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "try", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = TryMonadRules.ParseInput(number)
                .Bind(value =>
                {
                    try
                    {
                        return Right<string, decimal>(TryMonadRules.RiskyInverse(value));
                    }
                    catch (Exception ex)
                    {
                        return Left<string, decimal>(ex.Message);
                    }
                });

            result.Match(
                Right: inverse => _output.WriteLine($"Inverse: {inverse:0.####}"),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Try Monad Comparison");
}
