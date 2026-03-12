namespace Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;

public static class StreamingLargeDataRules
{
    public sealed record StreamAggregationResult(long ItemCount, long ChunkCount, decimal Total, decimal Average, decimal MaxChunkTotal);

    public static bool TryParseItemCount(string? value, out int itemCount, out string? error)
    {
        if (!int.TryParse(value, out itemCount))
        {
            error = "Item count must be an integer.";
            return false;
        }

        if (itemCount is < 1 or > 1_000_000)
        {
            error = "Item count must be between 1 and 1000000.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryParseChunkSize(string? value, out int chunkSize, out string? error)
    {
        if (!int.TryParse(value, out chunkSize))
        {
            error = "Chunk size must be an integer.";
            return false;
        }

        if (chunkSize is < 1 or > 100_000)
        {
            error = "Chunk size must be between 1 and 100000.";
            return false;
        }

        error = null;
        return true;
    }

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

    public static string FormatSummary(StreamAggregationResult result) =>
        $"Processed={result.ItemCount}, Chunks={result.ChunkCount}, Total={result.Total:0.##}, Average={result.Average:0.##}, MaxChunkTotal={result.MaxChunkTotal:0.##}";
}
