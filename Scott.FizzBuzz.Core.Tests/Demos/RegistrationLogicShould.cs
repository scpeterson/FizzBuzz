using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class RegistrationLogicShould
{
    [Fact]
    public void CSharpLogicReturnLeftWhenNameMissing()
    {
        var result = CSharpFunctionalRegistrationLogic.Register("", "21");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Name is required.");
    }

    [Fact]
    public void CSharpLogicReturnLeftWhenAgeIsNotNumeric()
    {
        var result = CSharpFunctionalRegistrationLogic.Register("Scott", "bad");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Age must be numeric.");
    }

    [Fact]
    public void CSharpLogicReturnRightForValidInput()
    {
        var result = CSharpFunctionalRegistrationLogic.Register("Scott", "21");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("Scott");
        result.Value.Age.Should().Be(21);
        result.Value.Id.Should().Be("scott-21");
    }

    [Fact]
    public void LanguageExtLogicReturnLeftWhenUnderage()
    {
        LanguageExtFunctionalRegistrationLogic.Register("Scott", "17")
            .ShouldBeLeft(error => error.Should().Be("Must be at least 18."));
    }

    [Fact]
    public void LanguageExtLogicReturnRightForValidInput()
    {
        LanguageExtFunctionalRegistrationLogic.Register("Scott", "21")
            .ShouldBeRight(user =>
            {
                user.Name.Should().Be("Scott");
                user.Age.Should().Be(21);
                user.Id.Should().Be("scott-21");
            });
    }
}
