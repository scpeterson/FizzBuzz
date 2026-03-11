using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.AsyncEffTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class NegativeDemoPathsShould
{
    [Fact]
    public void HandleInvalidNumberInImperativeAsyncWorkflowWithoutThrowing()
    {
        var output = new RecordingOutputSink();
        var demo = new ImperativeAsyncWorkflowDemo(output);

        Action act = () => _ = demo.Run("Scott", "bad");

        act.Should().NotThrow();
        _ = demo.Run("Scott", "bad");
        output.Messages.Should().Contain(message => message.Contains("Failed:", StringComparison.Ordinal));
    }

    [Fact]
    public void EmitLeftOutputForStrictJsonFailureScenarios()
    {
        var output = new RecordingOutputSink();
        var demo = new FpJsonStrictValidationDemo(output);

        _ = demo.Run(null, null);

        output.Messages.Should().Contain(message => message.Contains("Scenario: Unknown field", StringComparison.Ordinal));
        output.Messages.Should().Contain(message => message.Contains("Scenario: Duplicate property", StringComparison.Ordinal));
        output.Messages.Should().Contain(message => message.Contains("Left:", StringComparison.Ordinal));
    }

    [Fact]
    public void EmitLeftOutputForExtensionMembersInvalidInput()
    {
        var output = new RecordingOutputSink();
        var demo = new FpExtensionMembersTypeclassesDemo(output);

        _ = demo.Run(null, "1");

        output.Messages.Should().Contain(message =>
            message.Contains("Input '1': Value 2 is not a multiple of four.", StringComparison.Ordinal));
        output.Messages.Should().Contain(message =>
            message.Contains("Input 'abc': Could not parse 'abc' as an integer.", StringComparison.Ordinal));
    }
}
