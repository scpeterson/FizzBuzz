using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.OptionMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.OptionMonadTriad;

public class OptionMonadTriadShould
{
    [Fact]
    public void RunAllOptionMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        var demos = new IDemo[]
        {
            new ImperativeOptionComparisonDemo(output),
            new CSharpOptionComparisonDemo(output),
            new LanguageExtOptionMonadComparisonDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("complete", "21");
            result.ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForMissingOptionalChainInLanguageExtVariant()
    {
        var demo = new LanguageExtOptionMonadComparisonDemo();

        var result = demo.Run("no-profile", "21");

        result.ShouldBeLeft();
    }
}
