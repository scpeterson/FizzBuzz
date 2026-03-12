using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;
using Scott.FizzBuzz.Core.Interfaces;
using Scott.FizzBuzz.Core.Tests.TestUtilities;

namespace Scott.FizzBuzz.Core.Tests.Demos.ConfigurationValidationStartupTriad;

public class ConfigurationValidationStartupTriadShould
{
    [Fact]
    public void RunAllConfigurationValidationStartupTriadVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeConfigurationValidationStartupComparisonDemo(output),
            new CSharpConfigurationValidationStartupComparisonDemo(output),
            new LanguageExtConfigurationValidationStartupComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("prod", "20").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForNonNumericTimeoutInLanguageExtVariant() =>
        new LanguageExtConfigurationValidationStartupComparisonDemo().Run("prod", "abc").ShouldBeLeft();

    [Fact]
    public void AccumulateValidationErrorsInLanguageExtVariant()
    {
        var result = new LanguageExtConfigurationValidationStartupComparisonDemo().Run("misconfigured", "999");

        result.ShouldBeLeft(msg =>
        {
            msg.Should().Contain("Environment must be one of: dev|staging|prod.");
            msg.Should().Contain("Connection string must include Host and Database segments.");
            msg.Should().Contain("Timeout must be between 1 and 120 seconds.");
            msg.Should().Contain("Max retries must be between 0 and 10.");
        });
    }

    [Fact]
    public void ProduceEquivalentValidatedConfigsAcrossImplementations()
    {
        var imperative = ConfigurationValidationStartupRules.ExecuteImperative("staging", "25")
            .Config ?? throw new InvalidOperationException("Imperative config should be valid.");
        var csharp = ConfigurationValidationStartupRules.ExecuteCSharpPipeline("staging", "25")
            .Config ?? throw new InvalidOperationException("C# config should be valid.");
        var langExt = LanguageExtConfigurationValidationStartupRules.ExecuteLanguageExtPipeline("staging", "25")
            .IfLeft(error => throw new InvalidOperationException(error));

        Assert.Equal(imperative.Environment, csharp.Environment);
        Assert.Equal(imperative.Environment, langExt.Environment);
        Assert.Equal(imperative.ConnectionString, csharp.ConnectionString);
        Assert.Equal(imperative.ConnectionString, langExt.ConnectionString);
        Assert.Equal(imperative.TimeoutSeconds, csharp.TimeoutSeconds);
        Assert.Equal(imperative.TimeoutSeconds, langExt.TimeoutSeconds);
        Assert.Equal(imperative.MaxRetries, csharp.MaxRetries);
        Assert.Equal(imperative.MaxRetries, langExt.MaxRetries);
        Assert.Equal(imperative.CacheEnabled, csharp.CacheEnabled);
        Assert.Equal(imperative.CacheEnabled, langExt.CacheEnabled);
    }

    [Fact]
    public void EnforceProdTimeoutRuleAcrossImplementations()
    {
        ConfigurationValidationStartupRules.ExecuteImperative("prod", "45").IsSuccess.Should().BeFalse();
        ConfigurationValidationStartupRules.ExecuteCSharpPipeline("prod", "45").IsSuccess.Should().BeFalse();
        LanguageExtConfigurationValidationStartupRules.ExecuteLanguageExtPipeline("prod", "45").ShouldBeLeft();
    }
}
