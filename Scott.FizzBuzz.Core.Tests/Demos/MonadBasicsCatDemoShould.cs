using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class MonadBasicsCatDemoShould
{
    [Fact]
    public void ReturnRightAndDefaultToMiloWhenNameIsMissing()
    {
        var output = new RecordingOutput();
        var demo = new MonadBasicsCatDemo(output);

        var result = demo.Run(null, null);

        result.ShouldBeRight();
        output.Messages.Should().Contain("=== Imperative null + nested conditionals ===");
        output.Messages.Should().Contain("=== Functional Option/Either pipeline ===");
        output.Messages.Should().Contain(message => message.Contains("Milo: state is unknown.", StringComparison.Ordinal));
    }

    [Fact]
    public void EmitNotFoundMessageForUnknownCatInBothStyles()
    {
        var output = new RecordingOutput();
        var demo = new MonadBasicsCatDemo(output);

        var result = demo.Run("ghost", null);

        result.ShouldBeRight();
        output.Messages.Count(message => message.Contains("No cat found for 'ghost'.", StringComparison.Ordinal))
            .Should().Be(2);
    }

    [Fact]
    public void EmitAliveStateForKnownAliveCat()
    {
        var output = new RecordingOutput();
        var demo = new MonadBasicsCatDemo(output);

        var result = demo.Run("luna", null);

        result.ShouldBeRight();
        output.Messages.Count(message => message.Contains("Luna: alive.", StringComparison.Ordinal))
            .Should().Be(2);
    }

    private sealed class RecordingOutput : IOutput
    {
        public List<string> Messages { get; } = [];
        public void WriteLine(string message) => Messages.Add(message);
    }
}
