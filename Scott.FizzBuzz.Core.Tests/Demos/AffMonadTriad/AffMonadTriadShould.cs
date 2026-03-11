using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.AffMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.AffMonadTriad;

public class AffMonadTriadShould
{
    [Fact]
    public void RunAllAffMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos = [new ImperativeAffMonadComparisonDemo(output), new CSharpAffMonadComparisonDemo(output), new LanguageExtAffMonadComparisonDemo()];
        foreach (var demo in demos) demo.Run("scott", "7").ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForBadCountInLanguageExtVariant() =>
        new LanguageExtAffMonadComparisonDemo().Run("scott", "200").ShouldBeLeft();

}
