using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Tests.TestUtilities;
using Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.DomainWorkflowTriad;

public class DomainWorkflowTriadShould
{
    [Fact]
    public void RunAllDomainWorkflowVariantsForHappyPath()
    {
        var output = new NullOutputSink();
        IDemo[] demos =
        [
            new ImperativeDomainWorkflowComparisonDemo(output),
            new CSharpDomainWorkflowComparisonDemo(output),
            new LanguageExtDomainWorkflowComparisonDemo()
        ];

        foreach (var demo in demos)
        {
            demo.Run("scott", "21").ShouldBeRight();
        }
    }

    [Fact]
    public void ReturnLeftForAmountAboveApprovalLimitInLanguageExtVariant() =>
        new LanguageExtDomainWorkflowComparisonDemo().Run("scott", "999").ShouldBeLeft();
}
