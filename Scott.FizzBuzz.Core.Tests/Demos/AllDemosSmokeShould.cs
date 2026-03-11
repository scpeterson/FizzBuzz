using FluentAssertions;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class AllDemosSmokeShould
{
    [Fact]
    public void InstantiateAndRunAllCoreDemosWithoutThrowing()
    {
        var demoTypes = typeof(IDemo).Assembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                type.Namespace is not null &&
                type.Namespace.Contains(".Demos", StringComparison.Ordinal) &&
                typeof(IDemo).IsAssignableFrom(type))
            .OrderBy(type => type.FullName)
            .ToList();

        demoTypes.Should().NotBeEmpty();

        foreach (var demoType in demoTypes)
        {
            var demo = CreateDemo(demoType);
            demo.Key.Should().NotBeNullOrWhiteSpace();

            Action runTypicalInput = () => _ = demo.Run("Scott", "21");
            runTypicalInput.Should().NotThrow($"{demoType.Name} should execute for normal input");
        }
    }

    private static IDemo CreateDemo(Type demoType)
    {
        var outputCtor = demoType.GetConstructor([typeof(IOutput)]);
        if (outputCtor is not null)
        {
            return (IDemo)outputCtor.Invoke([new NullOutputSink()]);
        }

        var parameterlessCtor = demoType.GetConstructor(Type.EmptyTypes);
        if (parameterlessCtor is not null)
        {
            return (IDemo)parameterlessCtor.Invoke(null);
        }

        throw new InvalidOperationException($"Demo type '{demoType.FullName}' does not expose a supported constructor.");
    }
}
