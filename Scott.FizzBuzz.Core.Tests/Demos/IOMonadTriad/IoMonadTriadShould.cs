using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.IOMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.IOMonadTriad;

public class IoMonadTriadShould
{
    [Fact]
    public void RunAllIoMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        var demos = new IDemo[]
        {
            new ImperativeIoComparisonDemo(output),
            new CSharpIoComparisonDemo(output),
            new LanguageExtIoMonadComparisonDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("standard", "10");
            result.ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidWeightInLanguageExtVariant()
    {
        var demo = new LanguageExtIoMonadComparisonDemo();

        var result = demo.Run("standard", "abc");

        result.ShouldBeLeft();
    }
}
