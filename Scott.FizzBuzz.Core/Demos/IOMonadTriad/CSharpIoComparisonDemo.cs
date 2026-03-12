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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ResolveProfile(name)
                .Bind(profile => ParseWeight(number).Map(weight => (profile, weight, quote: IoMonadRules.CalculateQuote(weight, profile))));

            if (result.IsSuccess)
            {
                var tuple = result.Value;
                var audit = IoMonadRules.FormatAudit(tuple.profile, tuple.weight, tuple.quote);
                _output.WriteLine($"Result: quote = {tuple.quote:0.00}");
                _output.WriteLine($"Audit: {audit}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }

            _output.WriteLine("C#/.NET comparison note: side effects are explicit but still manually sequenced.");
        }, "C# IO Comparison");

    private static DemoResult<IoRuntimeProfile> ResolveProfile(string? name) =>
        IoMonadRules.TryResolveProfile(name, out var profile, out var error)
            ? DemoResult<IoRuntimeProfile>.Success(profile!)
            : DemoResult<IoRuntimeProfile>.Failure(error);

    private static DemoResult<decimal> ParseWeight(string? input) =>
        IoMonadRules.TryParseWeight(input, out var weight, out var error)
            ? DemoResult<decimal>.Success(weight)
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
