using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.TryMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.TryMonadTriad;

public class TryMonadTriadShould
{
    [Fact]
    public void RunAllTryMonadTriadVariantsForHappyPath()
    {
        var output = new NullOutput();
        IDemo[] demos = [new ImperativeTryMonadComparisonDemo(output), new CSharpTryMonadComparisonDemo(output), new LanguageExtTryMonadComparisonDemo()];
        foreach (var demo in demos) demo.Run("scott", "2").ShouldBeRight();
    }

    [Fact]
    public void ReturnLeftForZeroInLanguageExtVariant() =>
        new LanguageExtTryMonadComparisonDemo().Run("scott", "0").ShouldBeLeft();

    private sealed class NullOutput : IOutput { public void WriteLine(string message) { } }
}
