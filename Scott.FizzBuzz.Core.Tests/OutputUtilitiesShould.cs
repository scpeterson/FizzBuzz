using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests;

public class OutputUtilitiesShould
{
    [Fact]
    public void ExecuteWithSpacingUseStyledOutputWhenAvailable()
    {
        var output = new RecordingStyledOutputSink();
        var actionInvocations = 0;

        var result = OutputUtilities.ExecuteWithSpacing(
            output,
            () =>
            {
                actionInvocations++;
                output.WriteLine("action-body");
            },
            "DemoMethod");

        result.IsSuccess.Should().BeTrue();
        actionInvocations.Should().Be(1);
        output.Messages.Should().ContainInOrder(
            "************************************************************",
            "Executing method: DemoMethod",
            "************************************************************",
            "action-body",
            "************************************************************");
        output.Colors.Should().ContainInOrder(
            ConsoleColor.Cyan,
            ConsoleColor.Yellow,
            ConsoleColor.Cyan,
            ConsoleColor.Green,
            ConsoleColor.Cyan);
    }

    [Fact]
    public void ExecuteWithSpacingFallbackToPlainOutputWhenNotStyled()
    {
        var output = new RecordingOutputSink();
        var actionInvocations = 0;

        var result = OutputUtilities.ExecuteWithSpacing(
            output,
            () =>
            {
                actionInvocations++;
                output.WriteLine("action-body");
            },
            "DemoMethod");

        result.IsSuccess.Should().BeTrue();
        actionInvocations.Should().Be(1);
        output.Messages.Should().ContainInOrder(
            "************************************************************",
            "Executing method: DemoMethod",
            "************************************************************",
            "action-body",
            "************************************************************");
    }
}
