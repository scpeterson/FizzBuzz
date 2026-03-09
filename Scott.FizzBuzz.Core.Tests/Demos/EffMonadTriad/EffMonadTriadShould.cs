using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.EffMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.EffMonadTriad;

public class EffMonadTriadShould
{
    [Fact]
    public void RunAllEffMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutput();
        IDemo[] demos = [new ImperativeEffMonadComparisonDemo(output), new CSharpEffMonadComparisonDemo(output), new LanguageExtEffMonadComparisonDemo()];
        foreach (var demo in demos) demo.Run("scott", "21").ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForUnknownProfileInLanguageExtVariant() =>
        new LanguageExtEffMonadComparisonDemo().Run("unknown", "21").ShouldBeLeft();

    private sealed class NullOutput : IOutput { public void WriteLine(string message) { } }
}
