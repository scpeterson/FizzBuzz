using AutoFixture.Xunit2;
using FluentAssertions;
using LanguageExt;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.DotNet10Features;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Console.Tests;

public class DemoRunnerShould
{
    [Fact]
    public void ThrowArgumentExceptionWhenDemoKeysAreDuplicated()
    {
        // Arrange / Act
        Action ctor = () => _ = new DemoRunner([
            new StubDemo("duplicate-key", (_, _) => Right<string, Unit>(Unit.Default)),
            new StubDemo("duplicate-key", (_, _) => Right<string, Unit>(Unit.Default))
        ]);

        // Assert
        ctor.Should().Throw<ArgumentException>()
            .WithMessage("*Duplicate demo key(s): duplicate-key*");
    }

    [Fact]
    public void ThrowArgumentNullExceptionWhenInputIsNull()
    {
        // Arrange / Act
        Action ctor = () => _ = new DemoRunner(null!);

        // Assert
        ctor.Should().Throw<ArgumentNullException>()
            .WithParameterName("demos");
    }
    
    [Fact]
    public void ReturnLeftWhenOptsIsNull()
    {
        // Arrange: runner with an empty list of demos
        var runner = new DemoRunner([]);

        // Act
        var result = runner.Execute(null!);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be("opts was null"));
    }
    
    [Fact]
    public void ReturnLeftWhenNoMethodIsSpecified()
    {
        // Arrange: runner with no demos (so lookup doesn’t matter)
        var runner = new DemoRunner([]);
        var opts = new Options { Method = string.Empty, Name = null, Number = null };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be("No method specified"));
    }

    [Fact]
    public void ReturnLeftWhenTagIsUsedWithoutList()
    {
        // Arrange
        var runner = new DemoRunner([]);
        var opts = new Options { Method = "any", Tags = ["fp"] };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be("--tag can only be used with --list."));
    }

    [Fact]
    public void ReturnLeftWhenMethodIsUsedWithList()
    {
        // Arrange
        var runner = new DemoRunner([]);
        var opts = new Options { List = true, Method = "imperative" };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be("--method cannot be combined with --list."));
    }

    [Fact]
    public void ReturnLeftWhenNameOrNumberIsUsedWithList()
    {
        // Arrange
        var runner = new DemoRunner([]);
        var opts = new Options { List = true, Name = "scott", Number = "12" };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be("--name/--number cannot be combined with --list."));
    }

    [Fact]
    public void ListDemosWithoutTagFilters()
    {
        // Arrange
        var fpDemo = new StubDemo("fp-demo", (_, _) => Right<string, Unit>(Unit.Default));
        var imperativeDemo = new StubDemo("imperative-demo", (_, _) => Right<string, Unit>(Unit.Default));
        var runner = new DemoRunner([fpDemo, imperativeDemo]);
        var opts = new Options { List = true };
        var output = new StringWriter();
        var originalOut = System.Console.Out;

        try
        {
            System.Console.SetOut(output);

            // Act
            var result = runner.Execute(opts);

            // Assert
            result.ShouldBeRight();
            output.ToString().Should().Contain("fp-demo");
            output.ToString().Should().Contain("imperative-demo");
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ListDemosIncludeDescriptionWhenProvided()
    {
        // Arrange
        var describedDemo = new StubDemo(
            "described-demo",
            (_, _) => Right<string, Unit>(Unit.Default),
            description: "Demonstrates a described listing entry.");
        var runner = new DemoRunner([describedDemo]);
        var opts = new Options { List = true };
        var output = new StringWriter();
        var originalOut = System.Console.Out;

        try
        {
            System.Console.SetOut(output);

            // Act
            var result = runner.Execute(opts);

            // Assert
            result.ShouldBeRight();
            output.ToString().Should().Contain("description=Demonstrates a described listing entry.");
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ListDemosWithTagFilters()
    {
        // Arrange
        var fpDemo = new StubDemo(
            "fp-demo",
            (_, _) => Right<string, Unit>(Unit.Default),
            category: "functional",
            tags: ["fp", "dotnet10"]);
        var imperativeDemo = new StubDemo(
            "imperative-demo",
            (_, _) => Right<string, Unit>(Unit.Default),
            category: "imperative",
            tags: ["imperative"]);
        var runner = new DemoRunner([fpDemo, imperativeDemo]);
        var opts = new Options { List = true, Tags = ["fp"] };
        var output = new StringWriter();
        var originalOut = System.Console.Out;

        try
        {
            System.Console.SetOut(output);
            
            // Act
            var result = runner.Execute(opts);

            // Assert
            result.ShouldBeRight();
            output.ToString().Should().Contain("fp-demo");
            output.ToString().Should().NotContain("imperative-demo");
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ListDemosWithDotNet10TagShowsOnlyDotNet10Demos()
    {
        // Arrange
        var dotnet10JsonDemo = new FpJsonStrictValidationDemo();
        var dotnet10ExtensionDemo = new FpExtensionMembersTypeclassesDemo();
        var nonDotNet10Demo = new StubDemo(
            "legacy-demo",
            (_, _) => Right<string, Unit>(Unit.Default),
            category: "functional",
            tags: ["fp", "legacy"]);

        var runner = new DemoRunner([dotnet10JsonDemo, dotnet10ExtensionDemo, nonDotNet10Demo]);
        var opts = new Options { List = true, Tags = ["dotnet10"] };
        var output = new StringWriter();
        var originalOut = System.Console.Out;

        try
        {
            System.Console.SetOut(output);

            // Act
            var result = runner.Execute(opts);

            // Assert
            result.ShouldBeRight();
            output.ToString().Should().Contain("fp-json-strict-validation");
            output.ToString().Should().Contain("fp-extension-members-typeclasses");
            output.ToString().Should().NotContain("legacy-demo");
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }
    
    [Theory]
    [AutoData]
    public void ReturnRightForKnownDemo(string knownDemoName)
    {
        // Arrange: stub that always succeeds
        var stub = new StubDemo(knownDemoName, (_, _) => Right<string, Unit>(Unit.Default));
        var runner = new DemoRunner([stub]);
        var opts = new Options { Method = knownDemoName, Name = null, Number = null };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeRight(x => x.Should().BeAssignableTo<Unit>());
    }
    
    [Theory]
    [AutoData]
    public void ReturnsLeftWhenThereIsAnError(string knownDemoName, string name, string number, string error)
    {
        // Arrange: stub that fails
        var stub = new StubDemo(knownDemoName, (_, _) => Left<string, Unit>(error));
        var runner = new DemoRunner([stub]);
        var opts = new Options { Method = knownDemoName, Name = name, Number = number };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Be(error));
    }
    
    [Theory]
    [AutoData]
    public void ReturnsLeftForUnknownDemo(string unknownDemoName)
    {
        // Arrange: runner with no demos registered
        var runner = new DemoRunner([]);
        var opts = new Options { Method = unknownDemoName, Name = null, Number = null };

        // Act
        var result = runner.Execute(opts);

        // Assert
        result.ShouldBeLeft(msg => msg.Should().Contain("Unknown demo"));
    }
}
