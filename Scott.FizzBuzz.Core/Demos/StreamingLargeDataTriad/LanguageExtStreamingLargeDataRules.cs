using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public static class LanguageExtStreamingLargeDataRules
{
    private sealed record LanguageExtFoldState(long ItemCount, long ChunkCount, decimal Total, decimal CurrentChunkTotal, int CurrentChunkSize, decimal MaxChunkTotal);

    public static Either<string, int> ParseItemCount(string? value) =>
        StreamingLargeDataRules.TryParseItemCount(value, out var itemCount, out var error)
            ? Right<string, int>(itemCount)
            : Left<string, int>(error ?? "Item count must be an integer.");

    public static Either<string, int> ParseChunkSize(string? value) =>
        StreamingLargeDataRules.TryParseChunkSize(value, out var chunkSize, out var error)
            ? Right<string, int>(chunkSize)
            : Left<string, int>(error ?? "Chunk size must be an integer.");

    public static StreamingLargeDataRules.StreamAggregationResult ExecuteLanguageExtPipeline(int itemCount, int chunkSize)
    {
        var initial = new LanguageExtFoldState(0, 0, 0m, 0m, 0, 0m);

        var folded = Range(1, itemCount).Fold(initial, (state, index) =>
        {
            var measurement = StreamingLargeDataRules.MeasurementForIndex(index);
            var updated = state with
            {
                ItemCount = state.ItemCount + 1,
                Total = state.Total + measurement,
                CurrentChunkTotal = state.CurrentChunkTotal + measurement,
                CurrentChunkSize = state.CurrentChunkSize + 1
            };

            if (updated.CurrentChunkSize < chunkSize)
            {
                return updated;
            }

            return updated with
            {
                ChunkCount = updated.ChunkCount + 1,
                MaxChunkTotal = Math.Max(updated.MaxChunkTotal, updated.CurrentChunkTotal),
                CurrentChunkTotal = 0m,
                CurrentChunkSize = 0
            };
        });

        var finalized = folded.CurrentChunkSize > 0
            ? folded with
            {
                ChunkCount = folded.ChunkCount + 1,
                MaxChunkTotal = Math.Max(folded.MaxChunkTotal, folded.CurrentChunkTotal),
                CurrentChunkTotal = 0m,
                CurrentChunkSize = 0
            }
            : folded;

        var average = finalized.ItemCount == 0 ? 0m : finalized.Total / finalized.ItemCount;
        return new StreamingLargeDataRules.StreamAggregationResult(finalized.ItemCount, finalized.ChunkCount, finalized.Total, average, finalized.MaxChunkTotal);
    }
}
