using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public static class StreamingLargeDataRules
{
    public sealed record StreamAggregationResult(long ItemCount, long ChunkCount, decimal Total, decimal Average, decimal MaxChunkTotal);

    private sealed record LanguageExtFoldState(long ItemCount, long ChunkCount, decimal Total, decimal CurrentChunkTotal, int CurrentChunkSize, decimal MaxChunkTotal);

    public static Either<string, int> ParseItemCount(string? value) =>
        int.TryParse(value, out var parsed)
            ? parsed is >= 1 and <= 1_000_000
                ? Right<string, int>(parsed)
                : Left<string, int>("Item count must be between 1 and 1000000.")
            : Left<string, int>("Item count must be an integer.");

    public static Either<string, int> ParseChunkSize(string? value) =>
        int.TryParse(value, out var parsed)
            ? parsed is >= 1 and <= 100_000
                ? Right<string, int>(parsed)
                : Left<string, int>("Chunk size must be between 1 and 100000.")
            : Left<string, int>("Chunk size must be an integer.");

    public static int MeasurementForIndex(int oneBasedIndex) => ((oneBasedIndex * 37) % 100) + 1;

    public static IEnumerable<int> StreamMeasurements(int itemCount)
    {
        for (var index = 1; index <= itemCount; index++)
        {
            yield return MeasurementForIndex(index);
        }
    }

    public static StreamAggregationResult ExecuteImperative(int itemCount, int chunkSize)
    {
        long processed = 0;
        long chunks = 0;
        decimal total = 0m;
        decimal currentChunkTotal = 0m;
        var currentChunkSize = 0;
        decimal maxChunk = 0m;

        foreach (var measurement in StreamMeasurements(itemCount))
        {
            processed++;
            total += measurement;
            currentChunkTotal += measurement;
            currentChunkSize++;

            if (currentChunkSize == chunkSize)
            {
                chunks++;
                maxChunk = Math.Max(maxChunk, currentChunkTotal);
                currentChunkTotal = 0m;
                currentChunkSize = 0;
            }
        }

        if (currentChunkSize > 0)
        {
            chunks++;
            maxChunk = Math.Max(maxChunk, currentChunkTotal);
        }

        var average = processed == 0 ? 0m : total / processed;
        return new StreamAggregationResult(processed, chunks, total, average, maxChunk);
    }

    public static StreamAggregationResult ExecuteCSharpPipeline(int itemCount, int chunkSize)
    {
        var chunkTotals = StreamMeasurements(itemCount)
            .Chunk(chunkSize)
            .Select(chunk => chunk.Sum(value => (decimal)value));

        var summary = chunkTotals.Aggregate(
            seed: (ChunkCount: 0L, Total: 0m, MaxChunk: 0m),
            func: (state, chunkTotal) =>
                (state.ChunkCount + 1, state.Total + chunkTotal, Math.Max(state.MaxChunk, chunkTotal)));

        var average = summary.Total / itemCount;
        return new StreamAggregationResult(itemCount, summary.ChunkCount, summary.Total, average, summary.MaxChunk);
    }

    public static StreamAggregationResult ExecuteLanguageExtPipeline(int itemCount, int chunkSize)
    {
        var initial = new LanguageExtFoldState(ItemCount: 0, ChunkCount: 0, Total: 0m, CurrentChunkTotal: 0m, CurrentChunkSize: 0, MaxChunkTotal: 0m);

        var folded = Range(1, itemCount).Fold(initial, (state, index) =>
        {
            var measurement = MeasurementForIndex(index);
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
        return new StreamAggregationResult(finalized.ItemCount, finalized.ChunkCount, finalized.Total, average, finalized.MaxChunkTotal);
    }

    public static string FormatSummary(StreamAggregationResult result) =>
        $"Processed={result.ItemCount}, Chunks={result.ChunkCount}, Total={result.Total:0.##}, Average={result.Average:0.##}, MaxChunkTotal={result.MaxChunkTotal:0.##}";
}
