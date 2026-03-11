using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class MonadicFunctionsDemoShould
{
    [Fact]
    public void ReturnRightAndPrintAllComparisonSections()
    {
        var output = new RecordingOutputSink();
        var demo = new MonadicFunctionsDemo(output);

        var result = demo.Run(null, null);

        result.ShouldBeRight();
        output.Messages.Should().Contain("=== Imperative: hidden state ===");
        output.Messages.Should().Contain("=== Imperative: exception boundary ===");
        output.Messages.Should().Contain("=== Monadic Option: explicit failure, no exception ===");
        output.Messages.Should().Contain("=== Monadic Either: fail-fast with message ===");
    }

    [Fact]
    public void DemonstrateMonadicFailureAsDataNotException()
    {
        var output = new RecordingOutputSink();
        var demo = new MonadicFunctionsDemo(output);

        var result = demo.Run(null, null);

        result.ShouldBeRight();
        output.Messages.Should().Contain(message => message.StartsWith("Exception:", StringComparison.Ordinal));
        output.Messages.Should().Contain("No value because denominator was zero.");
        output.Messages.Should().Contain("Validation error: Must be less than 10.");
    }
}
