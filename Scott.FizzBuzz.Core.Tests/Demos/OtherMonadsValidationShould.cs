using FluentAssertions;
using LanguageExt.UnitTesting;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class OtherMonadsValidationShould
{
    [Fact]
    public void ReturnRightWhenFieldIsNotEmpty()
    {
        var result = OtherMonadsValidation.CheckNotEmpty("Email", "person@example.com");
        result.ShouldBeRight(value => value.Should().Be("person@example.com"));
    }

    [Fact]
    public void ReturnLeftWhenFieldIsEmpty()
    {
        var result = OtherMonadsValidation.CheckNotEmpty("Email", "   ");
        result.ShouldBeLeft(error => error.Should().Be("Email is required."));
    }

    [Fact]
    public void ReturnRightWhenMinimumLengthIsMet()
    {
        var result = OtherMonadsValidation.CheckMinLength("Password", "long-enough", 8);
        result.ShouldBeRight(value => value.Should().Be("long-enough"));
    }

    [Fact]
    public void ReturnLeftWhenMinimumLengthIsNotMet()
    {
        var result = OtherMonadsValidation.CheckMinLength("Password", "short", 8);
        result.ShouldBeLeft(error => error.Should().Be("Password must be at least 8 characters."));
    }

    [Fact]
    public void ValidateUserReturnRightForValidInput()
    {
        var result = OtherMonadsValidation.ValidateUser("person@example.com", "long-enough");
        result.ShouldBeRight(user =>
        {
            user.Email.Should().Be("person@example.com");
            user.Password.Should().Be("long-enough");
        });
    }

    [Fact]
    public void ValidateUserReturnLeftForInvalidInput()
    {
        var result = OtherMonadsValidation.ValidateUser("", "short");
        result.ShouldBeLeft(error => error.Should().Be("Email is required."));
    }
}
