using FluentAssertions;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class NewFunctionalDemosShould
{
    [Fact]
    public void RunJsonStrictValidationDemoSuccessfully()
    {
        // Arrange
        var demo = new FpJsonStrictValidationDemo();

        // Assert
        Action act = () => _ = demo.Run(name: null, number: null);
        act.Should().NotThrow();
    }

    [Fact]
    public void RunExtensionMembersTypeclassesDemoSuccessfully()
    {
        // Arrange
        var demo = new FpExtensionMembersTypeclassesDemo();

        // Assert
        Action act = () => _ = demo.Run(name: null, number: null);
        act.Should().NotThrow();
    }

    [Fact]
    public void ExposeDotNet10AndFpTags()
    {
        // Arrange
        var jsonDemo = new FpJsonStrictValidationDemo();
        var extensionsDemo = new FpExtensionMembersTypeclassesDemo();

        // Assert
        jsonDemo.Tags.Should().Contain(["fp", "dotnet10"]);
        extensionsDemo.Tags.Should().Contain(["fp", "dotnet10", "csharp14"]);
    }
}
