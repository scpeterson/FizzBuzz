using FluentAssertions;

namespace Scott.FizzBuzz.Console.Tests;

public static class DemoExecutionResultAssertions
{
    public static void ShouldBeRight(this DemoExecutionResult result)
    {
        result.IsSuccess.Should().BeTrue(result.Error);
    }

    public static void ShouldBeRight(this DemoExecutionResult result, Action<DemoExecutionResult> assertSuccess)
    {
        result.IsSuccess.Should().BeTrue(result.Error);
        assertSuccess(result);
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
