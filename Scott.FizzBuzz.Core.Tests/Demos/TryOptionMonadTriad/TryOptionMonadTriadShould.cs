using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.TryOptionMonadTriad;

public class TryOptionMonadTriadShould
{
    [Fact]
    public void RunAllTryOptionMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeTryOptionMonadComparisonDemo(output),
            new CSharpTryOptionMonadComparisonDemo(output),
            new LanguageExtTryOptionMonadComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("scott", "21").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForNoneInLanguageExtVariant() =>
        new LanguageExtTryOptionMonadComparisonDemo().Run("scott", "999").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForExceptionInLanguageExtVariant() =>
        new LanguageExtTryOptionMonadComparisonDemo().Run("scott", "13").ShouldBeLeft();
}
