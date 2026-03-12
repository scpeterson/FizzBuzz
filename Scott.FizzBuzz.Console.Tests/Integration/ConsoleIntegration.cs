using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Console.Tests.Integration;

public class ConsoleIntegration
{
    [Theory]
    [AutoData]
    public void Program_Wiring_AllowsDemoRunnerToResolveAndExecute(string methodName)
    {
        // Arrange: create a new ServiceCollection (instead of reusing Populate)
        var services = new ServiceCollection();
        // register the stub IDemo
        services.AddTransient<IDemo>(_ => new StubDemo(methodName, (_, __) => DemoExecutionResult.Success()));
        // register DemoRunner itself
        services.AddTransient<DemoRunner>();
            
        var provider = services.BuildServiceProvider();

        // Simulate what Program.cs does: ask for a DemoRunner from DI
        var runner = provider.GetRequiredService<DemoRunner>();

        // Act: call Execute with an Options whose Method = "foo"
        var opts = new Options { Method = methodName, Name = null, Number = null };
        var result = runner.Execute(opts);

        // Assert: since our StubDemo always succeeds, we should get success here
        result.ShouldBeRight();
    }

    [Theory]
    [AutoData]
    public void Program_Execute_WithInvalidMethod_YieldsLeftUnknownDemo(string unknownMethodName)
    {
        // Arrange: no IDemo registered at all
        var services = new ServiceCollection();
        services.AddTransient<DemoRunner>();
        var provider = services.BuildServiceProvider();
        var runner = provider.GetRequiredService<DemoRunner>();

        // Act
        var opts = new Options { Method = unknownMethodName, Name = null, Number = null };
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Contain("Unknown demo"));
    }
}
