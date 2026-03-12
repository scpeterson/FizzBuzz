using FluentAssertions;

namespace Scott.FizzBuzz.Core.Tests.TestUtilities;

public static class DemoExecutionResultAssertions
{
    public static void ShouldBeRight(this DemoExecutionResult result)
    {
        result.IsSuccess.Should().BeTrue(result.Error);
    }

    public static void ShouldBeLeft(this DemoExecutionResult result)
    {
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrWhiteSpace();
    }

    public static void ShouldBeLeft(this DemoExecutionResult result, Action<string> assertError)
    {
        result.IsSuccess.Should().BeFalse();
        assertError(result.Error!);
    }
}
