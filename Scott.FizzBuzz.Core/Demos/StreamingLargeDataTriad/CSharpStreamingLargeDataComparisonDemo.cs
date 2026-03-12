using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public class CSharpStreamingLargeDataComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpStreamingLargeDataComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpStreamingLargeDataComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-streaming-large-data-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "streaming", "large-data"];
    public string Description => "Chunked LINQ pipeline over an enumerable stream with compositional aggregation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseItemCount(name)
                .Bind(itemCount => ParseChunkSize(number).Map(chunkSize => StreamingLargeDataRules.ExecuteCSharpPipeline(itemCount, chunkSize)));

            if (result.IsSuccess)
            {
                _output.WriteLine($"Result: {StreamingLargeDataRules.FormatSummary(result.Value)}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }
        }, "C# Streaming / Large Data Comparison");

    private static DemoResult<int> ParseItemCount(string? value) =>
        StreamingLargeDataRules.TryParseItemCount(value, out var itemCount, out var error)
            ? DemoResult<int>.Success(itemCount)
            : DemoResult<int>.Failure(error);

    private static DemoResult<int> ParseChunkSize(string? value) =>
        StreamingLargeDataRules.TryParseChunkSize(value, out var chunkSize, out var error)
            ? DemoResult<int>.Success(chunkSize)
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
