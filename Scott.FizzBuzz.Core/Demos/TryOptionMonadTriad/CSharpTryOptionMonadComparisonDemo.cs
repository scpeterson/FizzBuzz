using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public class CSharpTryOptionMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpTryOptionMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpTryOptionMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-tryoption-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "tryoption", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var parsed = TryOptionMonadRules.ParseId(number);
            if (parsed.IsLeft)
            {
                _output.WriteLine($"Failed: {parsed.LeftToList()[0]}");
                return;
            }

            var id = parsed.RightToList()[0];
            try
            {
                var opt = TryOptionMonadRules.LookupOption(id);
                _output.WriteLine(opt.Match(
                    Some: value => $"Value found: {value:0.##}",
                    None: () => "No value for id."));
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "C# TryOption Monad Comparison");
}
