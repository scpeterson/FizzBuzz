using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.ValidationMonadTriad;

public class CSharpValidationMonadComparisonDemoShould
{
    [Fact]
    public void ExposeExpectedDemoMetadata()
    {
        var demo = new CSharpValidationMonadComparisonDemo();

        demo.Key.Should().Be("csharp-validation-monad-comparison");
        demo.Category.Should().Be("csharp");
        demo.Tags.Should().Contain(["fp", "csharp", "comparison", "validation", "monad"]);
        demo.Description.Should().Contain("error-list accumulation");
    }

    [Fact]
    public void AccumulateMultipleErrorsForInvalidNameAndAge()
    {
        var output = new RecordingOutput();
        var demo = new CSharpValidationMonadComparisonDemo(output);

        var result = demo.Run("1", "abc");

        result.ShouldBeRight();
        var combined = string.Join("\n", output.Messages);
        combined.Should().Contain("Name must be at least 3 characters.");
        combined.Should().Contain("Name must contain letters only.");
        combined.Should().Contain("Age must be numeric.");
        combined.Should().Contain("explicit List<string> accumulation");
    }

    [Fact]
    public void AccumulateRequiredNameAndAgeRangeErrors()
    {
        var output = new RecordingOutput();
        var demo = new CSharpValidationMonadComparisonDemo(output);

        var result = demo.Run("", "17");

        result.ShouldBeRight();
        var combined = string.Join("\n", output.Messages);
        combined.Should().Contain("Name is required.");
        combined.Should().Contain("Name must be at least 3 characters.");
        combined.Should().Contain("Age must be between 18 and 120.");
    }

    [Fact]
    public void EmitValidatedCandidateForHappyPath()
    {
        var output = new RecordingOutput();
        var demo = new CSharpValidationMonadComparisonDemo(output);

        var result = demo.Run("Scott", "21");

        result.ShouldBeRight();
        output.Messages.Should().Contain(message =>
            message.Contains("Validated candidate: Scott (21)", StringComparison.Ordinal));
        output.Messages.Should().Contain(message =>
            message.Contains("custom accumulation logic", StringComparison.Ordinal));
    }

    private sealed class RecordingOutput : IOutput
    {
        public List<string> Messages { get; } = [];

        public void WriteLine(string message) => Messages.Add(message);
    }
}
