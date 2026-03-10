using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.IdempotentCommandTriad;

public class IdempotentCommandTriadShould
{
    [Fact]
    public void RunAllIdempotentCommandVariantsForHappyPath()
    {
        var output = new NullOutput();
        IDemo[] demos =
        [
            new ImperativeIdempotentCommandComparisonDemo(output),
            new CSharpIdempotentCommandComparisonDemo(output),
            new LanguageExtIdempotentCommandComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("cmd-new", "21").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForDuplicateCommandInLanguageExtVariant() =>
        new LanguageExtIdempotentCommandComparisonDemo().Run("cmd-processed", "21").ShouldBeLeft();

    [Fact]
    public void ReturnLeftForInvalidAmountInLanguageExtVariant() =>
        new LanguageExtIdempotentCommandComparisonDemo().Run("cmd-new", "bad").ShouldBeLeft();

    private sealed class NullOutput : IOutput
    {
        public void WriteLine(string message)
        {
        }
    }
}
