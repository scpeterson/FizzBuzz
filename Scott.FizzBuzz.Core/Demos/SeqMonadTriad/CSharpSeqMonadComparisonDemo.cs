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
    public string Description => "Plain C# sequence pipeline using enumerable transforms before introducing Seq monad composition.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ResolveNumbers(name)
                .Bind(numbers => ParseThreshold(number).Map(threshold => numbers.Where(n => n >= threshold).Select(n => n * n).Sum()));

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: sum = {result.Value}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }
        }, "C# Seq Monad Comparison");

    private static DemoResult<IReadOnlyList<int>> ResolveNumbers(string? name) =>
        SeqMonadRules.TryResolveNumbers(name, out var numbers, out var error)
            ? DemoResult<IReadOnlyList<int>>.Success(numbers!)
            : DemoResult<IReadOnlyList<int>>.Failure(error);

    private static DemoResult<int> ParseThreshold(string? number) =>
        SeqMonadRules.TryParseThreshold(number, out var threshold, out var error)
            ? DemoResult<int>.Success(threshold)
            : DemoResult<int>.Failure(error);

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
