using FluentAssertions;
using LanguageExt.UnitTesting;
using AppValidationDemo = Scott.FizzBuzz.Core.ApplicativeValidationExample.ApplicativeValidationDemo;

namespace Scott.FizzBuzz.Core.Tests.Validation;

public class ApplicativeValidationDemoShould
{
    [Fact]
    public void ValidateToEntityReturnSuccessForValidInputs()
    {
        var result = AppValidationDemo.ValidateToEntity("Scott", "21");

        result.Match(
            Succ: entity =>
            {
                entity.FirstName.Should().Be("Scott");
                entity.Age.Should().Be(21);
            },
            Fail: _ => throw new Xunit.Sdk.XunitException("Expected success for valid inputs."));
    }

    [Fact]
    public void ValidateToEntityAccumulateFieldErrorsApplicatively()
    {
        var result = AppValidationDemo.ValidateToEntity("Scott1", "-2");

        result.ShouldBeFail();
        result.Match(
            Succ: _ => throw new Xunit.Sdk.XunitException("Expected validation failure."),
            Fail: errors =>
            {
                var messages = errors.Map(error => error.Message).ToArray();
                messages.Should().Contain(message => message.Contains("FirstName", StringComparison.Ordinal));
                messages.Should().Contain(message => message.Contains("Age", StringComparison.Ordinal));
            });
    }
}
