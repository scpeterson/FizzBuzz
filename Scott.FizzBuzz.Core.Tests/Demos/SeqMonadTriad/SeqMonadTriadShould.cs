using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.SeqMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.SeqMonadTriad;

public class SeqMonadTriadShould
{
    [Fact]
    public void RunAllSeqMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutput();
        IDemo[] demos = [new ImperativeSeqMonadComparisonDemo(output), new CSharpSeqMonadComparisonDemo(output), new LanguageExtSeqMonadComparisonDemo()];
        foreach (var demo in demos) demo.Run("scott", "3").ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForBadThresholdInLanguageExtVariant() =>
        new LanguageExtSeqMonadComparisonDemo().Run("scott", "abc").ShouldBeLeft();

    private sealed class NullOutput : IOutput { public void WriteLine(string message) { } }
}
