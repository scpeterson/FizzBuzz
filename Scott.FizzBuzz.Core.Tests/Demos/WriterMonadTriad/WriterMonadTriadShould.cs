using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.WriterMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.WriterMonadTriad;

public class WriterMonadTriadShould
{
    [Fact]
    public void RunAllWriterMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos = [new ImperativeWriterMonadComparisonDemo(output), new CSharpWriterMonadComparisonDemo(output), new LanguageExtWriterMonadComparisonDemo()];
        foreach (var demo in demos) demo.Run("scott", "10").ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForBadStartInLanguageExtVariant() =>
        new LanguageExtWriterMonadComparisonDemo().Run("scott", "abc").ShouldBeLeft();

}
