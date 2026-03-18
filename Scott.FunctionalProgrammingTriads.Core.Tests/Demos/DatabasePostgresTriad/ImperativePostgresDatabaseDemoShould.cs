using FluentAssertions;
using Scott.FunctionalProgrammingTriads.Core.Tests.TestUtilities;
using LanguageExt.UnitTesting;
using Scott.FunctionalProgrammingTriads.Core.Demos.DatabasePostgresTriad;
using Scott.FunctionalProgrammingTriads.Core.Interfaces;

namespace Scott.FunctionalProgrammingTriads.Core.Tests.Demos.DatabasePostgresTriad;

public class ImperativePostgresDatabaseDemoShould
{
    [Fact]
    public void EmitSkipMessageWhenConnectionStringIsMissing()
    {
        var output = new RecordingOutputSink();

        PostgresTestEnvironment.WithTempConnectionString(null, () =>
        {
            var result = new ImperativePostgresDatabaseDemo(output).Run("Scott", "21");

            result.ShouldBeRight();
            output.Messages.Should().Contain(message =>
                message.Contains("Skipping: set", StringComparison.Ordinal) &&
                message.Contains(PostgresDemoConfiguration.ConnectionEnvVar, StringComparison.Ordinal));
        });
    }

    [Fact]
    public void CatchParsingFailureForInvalidAgeInput()
    {
        var output = new RecordingOutputSink();

        PostgresTestEnvironment.WithTempConnectionString("Host=127.0.0.1;Port=1;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app;Timeout=1", () =>
        {
            var result = new ImperativePostgresDatabaseDemo(output).Run("Scott", "not-an-int");

            result.ShouldBeRight();
            output.Messages.Should().Contain(message =>
                message.Contains("Database operation failed:", StringComparison.Ordinal));
        });
    }

    [Fact]
    public void CatchConnectionFailureWhenConnectionStringIsUnusable()
    {
        var output = new RecordingOutputSink();

        PostgresTestEnvironment.WithTempConnectionString("Host=127.0.0.1;Port=1;Database=functional_programming_triads_demo;Username=functional_programming_triads_app;Password=functional_programming_triads_app;Timeout=1", () =>
        {
            var result = new ImperativePostgresDatabaseDemo(output).Run("Scott", "21");

            result.ShouldBeRight();
            output.Messages.Should().Contain(message =>
                message.Contains("Database operation failed:", StringComparison.Ordinal));
        });
    }

}
