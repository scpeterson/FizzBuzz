using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos;
using Scott.FizzBuzz.Core.Demos.EitherMonadTriad;
using Scott.FizzBuzz.Core.Demos.EffMonadTriad;
using Scott.FizzBuzz.Core.Demos.AffMonadTriad;
using Scott.FizzBuzz.Core.Demos.IOMonadTriad;
using Scott.FizzBuzz.Core.Demos.NullOptionTriad;
using Scott.FizzBuzz.Core.Demos.OptionMonadTriad;
using Scott.FizzBuzz.Core.Demos.ParseValidateTriad;
using Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;
using Scott.FizzBuzz.Core.Demos.SeqMonadTriad;
using Scott.FizzBuzz.Core.Demos.StateMonadTriad;
using Scott.FizzBuzz.Core.Demos.TryMonadTriad;
using Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;
using Scott.FizzBuzz.Core.Demos.ValidationAccumulationTriad;
using Scott.FizzBuzz.Core.Demos.WriterMonadTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class ScaffoldedTriadsShould
{
    [Fact]
    public void ExecuteScaffoldedTriadDemos()
    {
        // Placeholder safety net for scaffolded demos: verifies wiring + executable shape.
        var demos = new IDemo[]
        {
            new ImperativeNullHandlingDemo(),
            new CSharpNullPatternsDemo(),
            new LanguageExtOptionPipelineDemo(),
            new ImperativeOptionComparisonDemo(),
            new CSharpOptionComparisonDemo(),
            new LanguageExtOptionMonadComparisonDemo(),
            new ImperativeEitherComparisonDemo(),
            new CSharpEitherComparisonDemo(),
            new LanguageExtEitherMonadComparisonDemo(),
            new ImperativeValidationMonadComparisonDemo(),
            new CSharpValidationMonadComparisonDemo(),
            new LanguageExtValidationMonadComparisonDemo(),
            new ImperativeReaderComparisonDemo(),
            new CSharpReaderComparisonDemo(),
            new LanguageExtReaderMonadComparisonDemo(),
            new ImperativeStateComparisonDemo(),
            new CSharpStateComparisonDemo(),
            new LanguageExtStateMonadComparisonDemo(),
            new ImperativeIoComparisonDemo(),
            new CSharpIoComparisonDemo(),
            new LanguageExtIoMonadComparisonDemo(),
            new ImperativeTryMonadComparisonDemo(),
            new CSharpTryMonadComparisonDemo(),
            new LanguageExtTryMonadComparisonDemo(),
            new ImperativeEffMonadComparisonDemo(),
            new CSharpEffMonadComparisonDemo(),
            new LanguageExtEffMonadComparisonDemo(),
            new ImperativeAffMonadComparisonDemo(),
            new CSharpAffMonadComparisonDemo(),
            new LanguageExtAffMonadComparisonDemo(),
            new ImperativeSeqMonadComparisonDemo(),
            new CSharpSeqMonadComparisonDemo(),
            new LanguageExtSeqMonadComparisonDemo(),
            new ImperativeWriterMonadComparisonDemo(),
            new CSharpWriterMonadComparisonDemo(),
            new LanguageExtWriterMonadComparisonDemo(),
            new ImperativeParseValidateDemo(),
            new CSharpParseValidatePipelineDemo(),
            new LanguageExtEitherParseValidateDemo(),
            new ImperativeValidationFirstErrorDemo(),
            new CSharpValidationErrorListDemo(),
            new LanguageExtValidationAccumulateDemo()
        };

        foreach (var demo in demos)
        {
            var result = demo.Run("Scott", "21");
            result.ShouldBeRight();
        }
    }
}
