using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public class CSharpWriterMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpWriterMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpWriterMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-writer-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "writer", "monad"];
    public string Description => "Plain C# state-plus-log pipeline showing the manual plumbing that Writer later removes.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseStart(number)
                .Bind(start => ResolveOps(name).Map(ops =>
                    ops.Aggregate(
                        (State: start, Logs: new List<string>()),
                        (acc, op) =>
                        {
                            var next = WriterMonadRules.Step(acc.State, op);
                            acc.Logs.Add(next.LogEntry);
                            return (next.NextState, acc.Logs);
                        })));

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: final state = {result.Value.State}");
                result.Value.Logs.ForEach(_output.WriteLine);
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }
        }, "C# Writer Monad Comparison");

    private static DemoResult<int> ParseStart(string? number) =>
        WriterMonadRules.TryParseStart(number, out var start, out var error)
            ? DemoResult<int>.Success(start)
            : DemoResult<int>.Failure(error);

    private static DemoResult<IReadOnlyList<int>> ResolveOps(string? name) =>
        WriterMonadRules.TryResolveOps(name, out var ops, out var error)
            ? DemoResult<IReadOnlyList<int>>.Success(ops!)
            : DemoResult<IReadOnlyList<int>>.Failure(error);

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
