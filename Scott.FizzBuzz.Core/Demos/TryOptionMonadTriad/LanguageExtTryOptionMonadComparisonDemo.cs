using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public class LanguageExtTryOptionMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtTryOptionMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtTryOptionMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-tryoption-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "tryoption", "monad"];
    public string Description => "LanguageExt TryOption composition for lookups that can both fail and be absent.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt TryOption Monad Comparison",
            ComputeResult(number),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, string> ComputeResult(string? number)
    {
        var result =
            from id in LanguageExtTryOptionMonadRules.ParseId(number)
            select LanguageExtTryOptionMonadRules.LookupTryOption(id).Map(value => $"Some:{value:0.##}");

        return result.Bind(computation =>
        {
            var output = ifNoneOrFail(
                computation,
                None: () => "None",
                Fail: ex => $"Fail:{ex.Message}");

            if (output.StartsWith("Some:", StringComparison.Ordinal))
            {
                return Right<string, string>(output[5..]);
            }

            var message = output.StartsWith("Fail:", StringComparison.Ordinal)
                ? output[5..]
                : "No value for id.";

            return Left<string, string>(message);
        });
    }
}
