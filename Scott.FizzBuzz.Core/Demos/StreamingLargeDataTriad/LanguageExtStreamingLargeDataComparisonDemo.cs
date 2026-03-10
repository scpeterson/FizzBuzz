using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public class LanguageExtStreamingLargeDataComparisonDemo : IDemo
{
    public string Key => "langext-streaming-large-data-comparison";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "streaming", "large-data"];
    public string Description => "Pure fold-based stream policy in LanguageExt with deterministic, side-effect-free aggregation.";

    public Either<string, Unit> Run(string? name, string? number) =>
        StreamingLargeDataRules.ParseItemCount(name)
            .Bind(itemCount =>
                StreamingLargeDataRules.ParseChunkSize(number)
                    .Map(chunkSize => StreamingLargeDataRules.ExecuteLanguageExtPipeline(itemCount, chunkSize)))
            .Bind(summary =>
                summary.ItemCount > 0
                    ? Right<string, StreamingLargeDataRules.StreamAggregationResult>(summary)
                    : Left<string, StreamingLargeDataRules.StreamAggregationResult>("Stream did not process any items."))
            .Map(_ => unit);
}
