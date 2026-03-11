using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;

public class CSharpReaderComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpReaderComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpReaderComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-reader-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "reader", "monad"];
    public string Description => "C# functional composition still requires threading shared context through every function.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ReaderMonadRules.ResolveContext(name)
                .Bind(context =>
                    ReaderMonadRules.ParseSubtotal(number)
                        .Map(subtotal => ReaderMonadRules.ApplyTax(subtotal, context))
                        .Map(taxed => ReaderMonadRules.AddFee(taxed, context))
                        .Map(total => ReaderMonadRules.FormatTotal(total, context)));

            result.Match(
                Right: message => _output.WriteLine(message),
                Left: error => _output.WriteLine($"Failed: {error}"));

            _output.WriteLine("C#/.NET comparison note: context must be threaded through each function call.");
        }, "C# Reader Comparison");
}
