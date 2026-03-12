using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public class CSharpTryMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpTryMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpTryMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-try-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "try", "monad"];
    public string Description => "Plain C# explicit-result handling around risky calculations that might otherwise throw.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseInput(number).Bind(value =>
            {
                try
                {
                    return DemoResult<decimal>.Success(TryMonadRules.RiskyInverse(value));
                }
                catch (Exception ex)
                {
                    return DemoResult<decimal>.Failure(ex.Message);
                }
            });

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: inverse = {result.Value:0.####}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }
        }, "C# Try Monad Comparison");

    private static DemoResult<decimal> ParseInput(string? number) =>
        TryMonadRules.TryParseInput(number, out var value, out var error)
            ? DemoResult<decimal>.Success(value)
            : DemoResult<decimal>.Failure(error);

    private readonly record struct DemoResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DemoResult<T> Success(T value) => new(true, value, null);
        public static DemoResult<T> Failure(string? error) => new(false, default!, error);
        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess ? next(Value) : DemoResult<TNext>.Failure(Error);
    }
}
