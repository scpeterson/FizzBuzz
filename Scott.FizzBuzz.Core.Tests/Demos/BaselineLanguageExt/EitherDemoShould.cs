using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.BaselineLanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Tests.Demos.BaselineLanguageExt;

public class EitherDemoShould
{
    [Fact]
    public void RunShowErrorForInvalidInputAcrossImperativeAndFunctionalFlows()
    {
        var output = new RecordingOutputSink();
        var demo = new EitherDemo(output);

        var result = demo.Run("Scott", "11");

        result.ShouldBeRight();
        output.Messages.Should().Contain(message =>
            message.Contains("[Imperative] Error: Must be less than 10.", StringComparison.Ordinal));
        output.Messages.Should().Contain(message =>
            message.Contains("[Functional] Error: Must be less than 10.", StringComparison.Ordinal));
    }

    [Fact]
    public void RunShowValidForInputAcrossImperativeAndFunctionalFlows()
    {
        var output = new RecordingOutputSink();
        var demo = new EitherDemo(output);

        var result = demo.Run("Scott", "8");

        result.ShouldBeRight();
        output.Messages.Should().Contain(message =>
            message.Contains("[Imperative] Valid number: 8", StringComparison.Ordinal));
        output.Messages.Should().Contain(message =>
            message.Contains("[Functional] Valid number: 8", StringComparison.Ordinal));
    }

    [Fact]
    public void ValidateGreaterThanZeroNullableReturnFailForNone()
    {
        var result = EitherDemo.ValidateGreaterThanZeroNullable<int>(None, "age");

        result.Match(
            Succ: _ => throw new Xunit.Sdk.XunitException("Expected validation failure for None option."),
            Fail: errors => errors.Head.Message.Should().Contain("cannot be null"));
    }

    [Fact]
    public void ValidateGreaterThanZeroNullableReturnFailForZero()
    {
        var result = EitherDemo.ValidateGreaterThanZeroNullable(Some<int?>(0), "age");

        result.Match(
            Succ: _ => throw new Xunit.Sdk.XunitException("Expected validation failure for zero value."),
            Fail: errors => errors.Head.Message.Should().Contain("must be greater than zero"));
    }

    [Fact]
    public void ValidateGreaterThanZeroNullableReturnSuccessForPositiveValue()
    {
        var result = EitherDemo.ValidateGreaterThanZeroNullable(Some<int?>(5), "age");

        result.Match(
            Succ: value => value.Should().Be(5),
            Fail: errors => throw new Xunit.Sdk.XunitException($"Expected success but failed: {string.Join(" | ", errors.Map(error => error.Message))}"));
    }
}
