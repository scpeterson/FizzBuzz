using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.CurryingTriad;
using Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;
using Scott.FizzBuzz.Core.Demos.CompositionRootTriad;
using Scott.FizzBuzz.Core.Demos.DomainWorkflowTriad;
using Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;
using Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;
using Scott.FizzBuzz.Core.Demos.EitherMonadTriad;
using Scott.FizzBuzz.Core.Demos.EffMonadTriad;
using Scott.FizzBuzz.Core.Demos.AffMonadTriad;
using Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;
using Scott.FizzBuzz.Core.Demos.IOMonadTriad;
using Scott.FizzBuzz.Core.Demos.NullOptionTriad;
using Scott.FizzBuzz.Core.Demos.OptionMonadTriad;
using Scott.FizzBuzz.Core.Demos.ParseValidateTriad;
using Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;
using Scott.FizzBuzz.Core.Demos.StreamingLargeDataTriad;
using Scott.FizzBuzz.Core.Demos.RetryBackoffTriad;
using Scott.FizzBuzz.Core.Demos.SeqMonadTriad;
using Scott.FizzBuzz.Core.Demos.StateMonadTriad;
using Scott.FizzBuzz.Core.Demos.TryMonadTriad;
using Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;
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
            new ImperativeCurryingComparisonDemo(),
            new CSharpCurryingComparisonDemo(),
            new LanguageExtCurryingComparisonDemo(),
            new ImperativeCompositionRootComparisonDemo(),
            new CSharpCompositionRootComparisonDemo(),
            new LanguageExtCompositionRootComparisonDemo(),
            new ImperativeDomainWorkflowComparisonDemo(),
            new CSharpDomainWorkflowComparisonDemo(),
            new LanguageExtDomainWorkflowComparisonDemo(),
            new ImperativeEventSourcingLiteComparisonDemo(),
            new CSharpEventSourcingLiteComparisonDemo(),
            new LanguageExtEventSourcingLiteComparisonDemo(),
            new ImperativeConfigurationValidationStartupComparisonDemo(),
            new CSharpConfigurationValidationStartupComparisonDemo(),
            new LanguageExtConfigurationValidationStartupComparisonDemo(),
            new ImperativeConcurrencySafetyComparisonDemo(),
            new CSharpConcurrencySafetyComparisonDemo(),
            new LanguageExtConcurrencySafetyComparisonDemo(),
            new ImperativeStreamingLargeDataComparisonDemo(),
            new CSharpStreamingLargeDataComparisonDemo(),
            new LanguageExtStreamingLargeDataComparisonDemo(),
            new ImperativeRetryBackoffComparisonDemo(),
            new CSharpRetryBackoffComparisonDemo(),
            new LanguageExtRetryBackoffComparisonDemo(),
            new ImperativeIdempotentCommandComparisonDemo(),
            new CSharpIdempotentCommandComparisonDemo(),
            new LanguageExtIdempotentCommandComparisonDemo(),
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
            new ImperativeTryOptionMonadComparisonDemo(),
            new CSharpTryOptionMonadComparisonDemo(),
            new LanguageExtTryOptionMonadComparisonDemo(),
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
            var (name, number) = demo.Key.Contains("currying-comparison", StringComparison.Ordinal)
                ? ("vip", "100")
                : demo.Key.Contains("concurrency-safety-comparison", StringComparison.Ordinal)
                    ? ("scenario", "1000")
                : demo.Key.Contains("streaming-large-data-comparison", StringComparison.Ordinal)
                    ? ("1000", "128")
                : demo.Key.Contains("retry-backoff-comparison", StringComparison.Ordinal)
                    ? ("exp", "2")
                : demo.Key.Contains("startup-config-validation-comparison", StringComparison.Ordinal)
                    ? ("prod", "20")
                : ("Scott", "21");

            var result = demo.Run(name, number);
            result.ShouldBeRight();
        }
    }
}
