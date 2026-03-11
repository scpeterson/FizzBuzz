using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.ReaderMonadTriad;

public class ReaderMonadTriadShould
{
    [Fact]
    public void RunAllReaderMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        var demos = new IDemo[]
        {
            new ImperativeReaderComparisonDemo(output),
            new CSharpReaderComparisonDemo(output),
            new LanguageExtReaderMonadComparisonDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("vip", "50");
            result.ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForUnknownProfileInLanguageExtVariant()
    {
        var demo = new LanguageExtReaderMonadComparisonDemo();

        var result = demo.Run("unknown-profile", "50");

        result.ShouldBeLeft();
    }
}
