using LanguageExt;
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

    public string Key => "imperative-streaming-large-data-comparison";
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "streaming", "large-data"];
    public string Description => "Single-pass mutable loop over a stream to avoid materializing all records in memory.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            StreamingLargeDataRules.ParseItemCount(name).Match(
                Right: itemCount =>
                    StreamingLargeDataRules.ParseChunkSize(number).Match(
                        Right: chunkSize =>
                        {
                            var result = StreamingLargeDataRules.ExecuteImperative(itemCount, chunkSize);
                            _output.WriteLine(StreamingLargeDataRules.FormatSummary(result));
                        },
                        Left: error => _output.WriteLine($"Failed: {error}")),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "Imperative Streaming / Large Data Comparison");
}
