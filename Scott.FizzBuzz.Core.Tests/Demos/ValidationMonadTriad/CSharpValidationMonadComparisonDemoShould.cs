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

        demo.Key.Should().Be(CSharpValidationMonadComparisonDemo.DemoKey);
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
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.NameMinLengthMessage);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.NameLettersOnlyMessage);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.AgeNumericMessage);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.ErrorAccumulationNote);
    }

    [Fact]
    public void AccumulateRequiredNameAndAgeRangeErrors()
    {
        var output = new RecordingOutput();
        var demo = new CSharpValidationMonadComparisonDemo(output);

        var result = demo.Run("", "17");

        result.ShouldBeRight();
        var combined = string.Join("\n", output.Messages);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.NameRequiredMessage);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.NameMinLengthMessage);
        combined.Should().Contain(CSharpValidationMonadComparisonDemo.AgeRangeMessage);
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
        output.Messages.Should().Contain(CSharpValidationMonadComparisonDemo.SuccessAccumulationNote);
    }

    private sealed class RecordingOutput : IOutput
    {
        public List<string> Messages { get; } = [];

        public void WriteLine(string message) => Messages.Add(message);
    }
}
