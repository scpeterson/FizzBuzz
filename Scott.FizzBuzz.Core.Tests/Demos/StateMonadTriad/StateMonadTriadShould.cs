using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.StateMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.StateMonadTriad;

public class StateMonadTriadShould
{
    [Fact]
    public void RunAllStateMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        var demos = new IDemo[]
        {
            new ImperativeStateComparisonDemo(output),
            new CSharpStateComparisonDemo(output),
            new LanguageExtStateMonadComparisonDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("standard", "5");
            result.ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidStepInLanguageExtVariant()
    {
        var demo = new LanguageExtStateMonadComparisonDemo();

        var result = demo.Run("standard", "abc");

        result.ShouldBeLeft();
    }
}
