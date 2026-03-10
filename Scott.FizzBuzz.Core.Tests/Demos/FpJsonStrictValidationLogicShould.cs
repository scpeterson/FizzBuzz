using FluentAssertions;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class FpJsonStrictValidationLogicShould
{
    [Fact]
    public void ParseRequestStrictReturnRightForValidJson()
    {
        var result = FpJsonStrictValidationLogic.ParseRequestStrict("""{"name":"Ada","age":37}""");

        result.Match(
            Right: request =>
            {
                request.Name.Should().Be("Ada");
                request.Age.Should().Be(37);
            },
            Left: error => throw new Xunit.Sdk.XunitException($"Expected parse success, got: {error.Message}"));
    }

    [Fact]
    public void ParseRequestStrictReturnLeftForUnknownField()
    {
        var result = FpJsonStrictValidationLogic.ParseRequestStrict("""{"name":"Ada","age":37,"role":"admin"}""");

        result.Match(
            Right: _ => throw new Xunit.Sdk.XunitException("Expected parse failure for unknown field."),
            Left: error => error.Message.Should().Contain("Strict JSON parse failed"));
    }

    [Fact]
    public void ValidateNameReturnFailureForBlank()
    {
        var validation = FpJsonStrictValidationLogic.ValidateName("   ");

        validation.Match(
            Succ: _ => throw new Xunit.Sdk.XunitException("Expected name validation failure."),
            Fail: errors => errors.First().Message.Should().Be("Name is required."));
    }

    [Fact]
    public void ValidateAgeReturnFailureForUnderage()
    {
        var validation = FpJsonStrictValidationLogic.ValidateAge(12);

        validation.Match(
            Succ: _ => throw new Xunit.Sdk.XunitException("Expected age validation failure."),
            Fail: errors => errors.First().Message.Should().Contain("Age must be >= 18"));
    }

    [Fact]
    public void ValidateAdultAccumulateToSuccessForValidRequest()
    {
        var validation = FpJsonStrictValidationLogic.ValidateAdult(new CreateUserRequest("Ada", 37));

        validation.Match(
            Succ: adult =>
            {
                adult.Name.Should().Be("Ada");
                adult.Age.Should().Be(37);
            },
            Fail: _ => throw new Xunit.Sdk.XunitException("Expected adult validation success."));
    }
}
