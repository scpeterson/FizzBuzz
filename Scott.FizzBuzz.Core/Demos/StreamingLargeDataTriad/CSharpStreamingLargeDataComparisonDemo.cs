using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = StreamingLargeDataRules.ParseItemCount(name)
                .Bind(itemCount =>
                    StreamingLargeDataRules.ParseChunkSize(number)
                        .Map(chunkSize => StreamingLargeDataRules.ExecuteCSharpPipeline(itemCount, chunkSize)));

            result.Match(
                Right: summary => _output.WriteLine(StreamingLargeDataRules.FormatSummary(summary)),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Streaming / Large Data Comparison");
}
