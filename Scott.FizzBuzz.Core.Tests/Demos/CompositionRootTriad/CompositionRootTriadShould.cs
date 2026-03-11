using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.CompositionRootTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.CompositionRootTriad;

public class CompositionRootTriadShould
{
    [Fact]
    public void RunAllCompositionRootVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeCompositionRootComparisonDemo(output),
            new CSharpCompositionRootComparisonDemo(output),
            new LanguageExtCompositionRootComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("vip", "100").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidAmountInLanguageExtVariant() =>
        new LanguageExtCompositionRootComparisonDemo().Run("vip", "abc").ShouldBeLeft();
}
