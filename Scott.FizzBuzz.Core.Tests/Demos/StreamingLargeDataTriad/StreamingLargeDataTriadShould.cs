using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.StreamingLargeDataTriad;

public class StreamingLargeDataTriadShould
{
    [Fact]
    public void RunAllStreamingLargeDataTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeStreamingLargeDataComparisonDemo(output),
            new CSharpStreamingLargeDataComparisonDemo(output),
            new LanguageExtStreamingLargeDataComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("1000", "128").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidCountInLanguageExtVariant() =>
        new LanguageExtStreamingLargeDataComparisonDemo().Run("bad", "128").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForInvalidChunkSizeInLanguageExtVariant() =>
        new LanguageExtStreamingLargeDataComparisonDemo().Run("1000", "0").ShouldBeLeft();

    [Fact]
    public void ProduceEquivalentTotalsAcrossImplementations()
    {
        const int itemCount = 5000;
        const int chunkSize = 250;

        var imperative = StreamingLargeDataRules.ExecuteImperative(itemCount, chunkSize);
        var csharp = StreamingLargeDataRules.ExecuteCSharpPipeline(itemCount, chunkSize);
        var langExt = LanguageExtStreamingLargeDataRules.ExecuteLanguageExtPipeline(itemCount, chunkSize);

        Assert.Equal(imperative.ItemCount, csharp.ItemCount);
        Assert.Equal(imperative.ItemCount, langExt.ItemCount);
        Assert.Equal(imperative.ChunkCount, csharp.ChunkCount);
        Assert.Equal(imperative.ChunkCount, langExt.ChunkCount);
        Assert.Equal(imperative.Total, csharp.Total);
        Assert.Equal(imperative.Total, langExt.Total);
        Assert.Equal(imperative.MaxChunkTotal, csharp.MaxChunkTotal);
        Assert.Equal(imperative.MaxChunkTotal, langExt.MaxChunkTotal);
    }
}
