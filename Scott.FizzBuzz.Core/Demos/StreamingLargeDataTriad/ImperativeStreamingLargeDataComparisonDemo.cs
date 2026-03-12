using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public class ImperativeStreamingLargeDataComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeStreamingLargeDataComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeStreamingLargeDataComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-streaming-large-data-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "streaming", "large-data"];
    public string Description => "Single-pass mutable loop over a stream to avoid materializing all records in memory.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!StreamingLargeDataRules.TryParseItemCount(name, out var itemCount, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!StreamingLargeDataRules.TryParseChunkSize(number, out var chunkSize, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var result = StreamingLargeDataRules.ExecuteImperative(itemCount, chunkSize);
            _output.WriteLine($"Result: {StreamingLargeDataRules.FormatSummary(result)}");
        }, "Imperative Streaming / Large Data Comparison");
}
