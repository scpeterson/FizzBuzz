using FluentAssertions;
using LanguageExt.UnitTesting;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class EitherValidationShould
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ImperativeValidateReturnRequiredErrorWhenInputMissing(string? input)
    {
        var result = EitherValidation.ImperativeValidate(input);
        result.ShouldBeLeft(error => error.Should().Be("Number is required."));
    }

    [Fact]
    public void ImperativeValidateReturnParseErrorWhenInputNotInteger()
    {
        var result = EitherValidation.ImperativeValidate("abc");
        result.ShouldBeLeft(error => error.Should().Be("Not a valid integer."));
    }

    [Fact]
    public void ImperativeValidateReturnRangeErrorWhenInputTooLarge()
    {
        var result = EitherValidation.ImperativeValidate("10");
        result.ShouldBeLeft(error => error.Should().Be("Must be less than 10."));
    }

    [Fact]
    public void ImperativeValidateReturnRightWhenInputValid()
    {
        var result = EitherValidation.ImperativeValidate("9");
        result.ShouldBeRight(value => value.Should().Be(9));
    }

    [Fact]
    public void FunctionalValidateMatchImperativeBehaviorForFailureAndSuccess()
    {
        EitherValidation.FunctionalValidate("abc")
            .ShouldBeLeft(error => error.Should().Be("Not a valid integer."));

        EitherValidation.FunctionalValidate("9")
            .ShouldBeRight(value => value.Should().Be(9));
    }

    [Fact]
    public void CheckLessThanReturnExpectedBranch()
    {
        var check = EitherValidation.CheckLessThan(10);

        check(8).ShouldBeRight(value => value.Should().Be(8));
        check(10).ShouldBeLeft(error => error.Should().Be("Must be less than 10."));
    }
}
