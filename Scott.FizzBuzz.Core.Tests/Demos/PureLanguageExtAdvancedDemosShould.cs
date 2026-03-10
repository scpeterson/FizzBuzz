using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.AsyncEffTriad;
using Scott.FizzBuzz.Core.Demos.CollectionsAggregationTriad;
using Scott.FizzBuzz.Core.Demos.DatabaseTextStoreTriad;
using Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;
using Scott.FizzBuzz.Core.Demos.ExceptionBoundariesTriad;

namespace Scott.FizzBuzz.Core.Tests.Demos;

public class PureLanguageExtAdvancedDemosShould
{
    [Fact]
    public void ReturnPureValuesForSuccessAndFailureCases()
    {
        // Collections + Aggregation
        new LanguageExtSeqAggregationDemo().Run("Scott", "21").ShouldBeRight();

        // Exception Boundaries
        new LanguageExtTryEitherRecoveryDemo().Run("Scott", "5").ShouldBeRight();
        new LanguageExtTryEitherRecoveryDemo().Run("Scott", "0").ShouldBeLeft();

        // Async + Eff
        new LanguageExtAsyncEffWorkflowDemo().Run("Scott", "10").ShouldBeRight();
        new LanguageExtAsyncEffWorkflowDemo().Run("Scott", "bad").ShouldBeLeft();

        // End-to-End Mini Feature
        new LanguageExtFunctionalRegistrationDemo().Run("Scott", "21").ShouldBeRight();
        new LanguageExtFunctionalRegistrationDemo().Run("", "bad").ShouldBeLeft();

        // Database text-store side-effect boundary
        new LanguageExtEffTextStoreDatabaseDemo().Run("Scott", "21").ShouldBeRight();
        new LanguageExtEffTextStoreDatabaseDemo().Run("Scott", "bad").ShouldBeLeft();
    }
}
