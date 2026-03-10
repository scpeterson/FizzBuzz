using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class ApplicativeValidationDemoEntryShould
{
    [Fact]
    public void ReturnRightForValidInput()
    {
        var demo = new ApplicativeValidationDemo(new RecordingOutput());

        var result = demo.Run("Scott", "21");

        result.ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForInvalidInput()
    {
        var demo = new ApplicativeValidationDemo(new RecordingOutput());

        var result = demo.Run("Scott1", "-2");

        result.ShouldBeLeft();
    }

    private sealed class RecordingOutput : IOutput
    {
        public List<string> Messages { get; } = [];

        public void WriteLine(string message) => Messages.Add(message);
    }
}
