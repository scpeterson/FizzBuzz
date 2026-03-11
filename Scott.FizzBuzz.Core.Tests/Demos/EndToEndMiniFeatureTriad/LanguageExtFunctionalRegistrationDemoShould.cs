using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

namespace Scott.FizzBuzz.Core.Tests.Demos.EndToEndMiniFeatureTriad;

public class LanguageExtFunctionalRegistrationDemoShould
{
    [Fact]
    public void ExposeExpectedDemoMetadata()
    {
        var demo = new LanguageExtFunctionalRegistrationDemo();

        demo.Key.Should().Be(LanguageExtFunctionalRegistrationDemo.DemoKey);
        demo.Category.Should().Be("functional");
        demo.Tags.Should().Contain(["fp", "languageext", "comparison", "end-to-end"]);
        demo.Description.Should().Contain("LanguageExt registration flow");
    }

    [Fact]
    public void ReturnRightForValidInput() =>
        new LanguageExtFunctionalRegistrationDemo().Run("Scott", "21").ShouldBeRight();

    [Fact]
    public void ReturnLeftForMissingName() =>
        new LanguageExtFunctionalRegistrationDemo().Run("", "21").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForUnderageInput() =>
        new LanguageExtFunctionalRegistrationDemo().Run("Scott", "17").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForNonNumericAge() =>
        new LanguageExtFunctionalRegistrationDemo().Run("Scott", "bad").ShouldBeLeft();
}
