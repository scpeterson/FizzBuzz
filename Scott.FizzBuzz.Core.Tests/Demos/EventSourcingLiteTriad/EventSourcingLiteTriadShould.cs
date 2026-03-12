using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.EventSourcingLiteTriad;

public class EventSourcingLiteTriadShould
{
    [Fact]
    public void RunAllEventSourcingLiteTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeEventSourcingLiteComparisonDemo(output),
            new CSharpEventSourcingLiteComparisonDemo(output),
            new LanguageExtEventSourcingLiteComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("acct-123", "25").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidAmountInLanguageExtVariant() =>
        new LanguageExtEventSourcingLiteComparisonDemo().Run("acct-123", "bad").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForMissingStreamIdInLanguageExtVariant() =>
        new LanguageExtEventSourcingLiteComparisonDemo().Run("", "25").ShouldBeLeft();

    [Fact]
    public void ProduceEquivalentResultsAcrossImplementationsForExistingStream()
    {
        const string streamId = "acct-123";
        const int deposit = 25;

        var imperative = EventSourcingLiteRules.ExecuteImperative(streamId, deposit);
        var csharp = EventSourcingLiteRules.ExecuteCSharpPipeline(streamId, deposit);
        var langExt = LanguageExtEventSourcingLiteRules.ExecuteLanguageExtPipeline(streamId, deposit);

        Assert.Equal(imperative.After.Balance, csharp.After.Balance);
        Assert.Equal(imperative.After.Balance, langExt.After.Balance);
        Assert.Equal(imperative.Delta, csharp.Delta);
        Assert.Equal(imperative.Delta, langExt.Delta);
        Assert.Equal(imperative.EventCountAfter, csharp.EventCountAfter);
        Assert.Equal(imperative.EventCountAfter, langExt.EventCountAfter);
    }

    [Fact]
    public void AutoOpenNewStreamAcrossImplementations()
    {
        const string streamId = "new-stream";
        const int deposit = 10;

        var imperative = EventSourcingLiteRules.ExecuteImperative(streamId, deposit);
        var csharp = EventSourcingLiteRules.ExecuteCSharpPipeline(streamId, deposit);
        var langExt = LanguageExtEventSourcingLiteRules.ExecuteLanguageExtPipeline(streamId, deposit);

        Assert.True(imperative.After.Opened);
        Assert.True(csharp.After.Opened);
        Assert.True(langExt.After.Opened);
        Assert.Equal(2, imperative.EventCountAfter);
        Assert.Equal(2, csharp.EventCountAfter);
        Assert.Equal(2, langExt.EventCountAfter);
        Assert.Equal(deposit, imperative.After.Balance);
        Assert.Equal(deposit, csharp.After.Balance);
        Assert.Equal(deposit, langExt.After.Balance);
    }
}
