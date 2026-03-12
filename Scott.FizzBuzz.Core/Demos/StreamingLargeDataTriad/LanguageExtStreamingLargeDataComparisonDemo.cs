using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public class LanguageExtStreamingLargeDataComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtStreamingLargeDataComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtStreamingLargeDataComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-streaming-large-data-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "streaming", "large-data"];
    public string Description => "Pure fold-based stream policy in LanguageExt with deterministic, side-effect-free aggregation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Streaming Large Data Comparison",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: {StreamingLargeDataRules.FormatSummary(result)}"));

    private static Either<string, StreamingLargeDataRules.StreamAggregationResult> ComputeResult(string? name, string? number) =>
        LanguageExtStreamingLargeDataRules.ParseItemCount(name)
            .Bind(itemCount =>
                LanguageExtStreamingLargeDataRules.ParseChunkSize(number)
                    .Map(chunkSize => LanguageExtStreamingLargeDataRules.ExecuteLanguageExtPipeline(itemCount, chunkSize)))
            .Bind(summary =>
                summary.ItemCount > 0
                    ? Right<string, StreamingLargeDataRules.StreamAggregationResult>(summary)
                    : Left<string, StreamingLargeDataRules.StreamAggregationResult>("Stream did not process any items."));
}
