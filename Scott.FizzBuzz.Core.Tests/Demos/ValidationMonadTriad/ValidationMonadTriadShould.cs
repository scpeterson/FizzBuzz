using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.ValidationMonadTriad;

public class ValidationMonadTriadShould
{
    [Fact]
    public void RunAllValidationMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();

        var demos = new IDemo[]
        {
            new ImperativeValidationMonadComparisonDemo(output),
            new CSharpValidationMonadComparisonDemo(output),
            new LanguageExtValidationMonadComparisonDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("Scott", "21");
            result.ShouldBeRight();
        }
    }

    [Fact]
    public void AccumulateMultipleValidationErrorsInLanguageExtVariant()
    {
        var demo = new LanguageExtValidationMonadComparisonDemo();

        var result = demo.Run("1", "abc");

        result.ShouldBeLeft(msg =>
        {
            msg.Should().Contain("Name must be at least 3 characters.");
            msg.Should().Contain("Name must contain letters only.");
            msg.Should().Contain("Age must be numeric.");
        });
    }
}
