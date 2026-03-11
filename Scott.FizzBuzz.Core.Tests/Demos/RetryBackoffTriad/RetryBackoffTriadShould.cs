using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.RetryBackoffTriad;

public class RetryBackoffTriadShould
{
    [Fact]
    public void RunAllRetryBackoffTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        IDemo[] demos =
        [
            new ImperativeRetryBackoffComparisonDemo(output),
            new CSharpRetryBackoffComparisonDemo(output),
            new LanguageExtRetryBackoffComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("exp", "2").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForInvalidPolicyInLanguageExtVariant() =>
        new LanguageExtRetryBackoffComparisonDemo().Run("bogus", "2").ShouldBeLeft();

    [Fact]
    public void ReturnLeftWhenRetriesExhaustInLanguageExtVariant() =>
        new LanguageExtRetryBackoffComparisonDemo().Run("linear", "10").ShouldBeLeft();

    [Fact]
    public void CalculateExpectedExponentialDelays()
    {
        var policy = RetryBackoffRules.ResolvePolicy("exp").IfLeft(error => throw new InvalidOperationException(error));

        Assert.Equal(100d, RetryBackoffRules.DelayForAttempt(policy, 1).TotalMilliseconds);
        Assert.Equal(200d, RetryBackoffRules.DelayForAttempt(policy, 2).TotalMilliseconds);
        Assert.Equal(400d, RetryBackoffRules.DelayForAttempt(policy, 3).TotalMilliseconds);
    }

    [Fact]
    public void ProduceDeterministicLanguageExtSchedule()
    {
        var policy = RetryBackoffRules.ResolvePolicy("exp").IfLeft(error => throw new InvalidOperationException(error));

        var execution = RetryBackoffRules.ExecuteLanguageExtPipeline(policy, failuresBeforeSuccess: 2);

        Assert.True(execution.Success);
        Assert.Equal(3, execution.Attempts);
        Assert.Equal([100d, 200d], execution.BackoffSchedule.Select(delay => delay.TotalMilliseconds).ToArray());
    }
}
