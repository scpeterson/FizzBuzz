using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class ApplicativeValidationDemoEntryShould
{
    [Fact]
    public void ReturnRightForValidInput()
    {
        var demo = new ApplicativeValidationDemo(new RecordingOutputSink());

        var result = demo.Run("Scott", "21");

        result.ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForInvalidInput()
    {
        var demo = new ApplicativeValidationDemo(new RecordingOutputSink());

        var result = demo.Run("Scott1", "-2");

        result.ShouldBeLeft();
    }
}
