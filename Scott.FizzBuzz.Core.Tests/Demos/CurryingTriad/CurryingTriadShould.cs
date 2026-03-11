using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.CurryingTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.CurryingTriad;

public class CurryingTriadShould
{
    [Fact]
    public void RunAllCurryingTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeCurryingComparisonDemo(output),
            new CSharpCurryingComparisonDemo(output),
            new LanguageExtCurryingComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("vip", "100").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidTierInLanguageExtVariant() =>
        new LanguageExtCurryingComparisonDemo().Run("unknown", "100").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForInvalidAmountInLanguageExtVariant() =>
        new LanguageExtCurryingComparisonDemo().Run("vip", "abc").ShouldBeLeft();
}
