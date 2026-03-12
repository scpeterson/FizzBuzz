using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public class CSharpEventSourcingLiteComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpEventSourcingLiteComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpEventSourcingLiteComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-event-sourcing-lite-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "event-sourcing", "state", "database"];
    public string Description => "Pipeline-style replay and append with immutable projections using C# pattern matching and aggregation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseStreamId(name)
                .Bind(streamId => ParseDepositAmount(number).Map(depositAmount => EventSourcingLiteRules.ExecuteCSharpPipeline(streamId, depositAmount)));

            if (result.IsSuccess)
            {
                _output.WriteLine("Result: event stream updated.");
                _output.WriteLine(EventSourcingLiteRules.FormatSummary(result.Value));
                return;
            }

            _output.WriteLine($"Failed: {result.Error}");
        }, "C# Event Sourcing Lite Comparison");

    private static DemoResult<string> ParseStreamId(string? value) =>
        EventSourcingLiteRules.TryParseStreamId(value, out var streamId, out var error)
            ? DemoResult<string>.Success(streamId!)
            : DemoResult<string>.Failure(error);

    private static DemoResult<int> ParseDepositAmount(string? value) =>
        EventSourcingLiteRules.TryParseDepositAmount(value, out var amount, out var error)
            ? DemoResult<int>.Success(amount)
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
