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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ResolveContext(name)
                .Bind(context =>
                    ParseSubtotal(number)
                        .Map(subtotal => ReaderMonadRules.ApplyTax(subtotal, context))
                        .Map(taxed => ReaderMonadRules.AddFee(taxed, context))
                        .Map(total => ReaderMonadRules.FormatTotal(total, context)));

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: {result.Value}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }

            _output.WriteLine("C#/.NET comparison note: context must be threaded through each function call.");
        }, "C# Reader Comparison");

    private static DemoResult<ReaderPricingContext> ResolveContext(string? name) =>
        ReaderMonadRules.TryResolveContext(name, out var context, out var error)
            ? DemoResult<ReaderPricingContext>.Success(context!)
            : DemoResult<ReaderPricingContext>.Failure(error);

    private static DemoResult<decimal> ParseSubtotal(string? input) =>
        ReaderMonadRules.TryParseSubtotal(input, out var subtotal, out var error)
            ? DemoResult<decimal>.Success(subtotal)
            : DemoResult<decimal>.Failure(error);

    private readonly record struct DemoResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DemoResult<T> Success(T value) => new(true, value, null);
        public static DemoResult<T> Failure(string? error) => new(false, default!, error);
        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess ? next(Value) : DemoResult<TNext>.Failure(Error);
        public DemoResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess ? DemoResult<TNext>.Success(map(Value)) : DemoResult<TNext>.Failure(Error);
    }
}
