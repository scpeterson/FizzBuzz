using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.Demos.DatabasePostgresTriad;

public class ImperativePostgresDatabaseDemoShould
{
    [Fact]
    public void EmitSkipMessageWhenConnectionStringIsMissing()
    {
        var output = new RecordingOutput();

        WithTempConnectionString(null, () =>
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
        var output = new RecordingOutput();

        WithTempConnectionString("Host=127.0.0.1;Port=1;Database=fizzbuzz;Username=fizzbuzz_app;Password=fizzbuzz_app;Timeout=1", () =>
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
        var output = new RecordingOutput();

        WithTempConnectionString("Host=127.0.0.1;Port=1;Database=fizzbuzz;Username=fizzbuzz_app;Password=fizzbuzz_app;Timeout=1", () =>
        {
            var result = new ImperativePostgresDatabaseDemo(output).Run("Scott", "21");

            result.ShouldBeRight();
            output.Messages.Should().Contain(message =>
                message.Contains("Database operation failed:", StringComparison.Ordinal));
        });
    }

    private static void WithTempConnectionString(string? value, Action action)
    {
        var original = Environment.GetEnvironmentVariable(PostgresDemoConfiguration.ConnectionEnvVar);

        try
        {
            Environment.SetEnvironmentVariable(PostgresDemoConfiguration.ConnectionEnvVar, value);
            action();
        }
        finally
        {
            Environment.SetEnvironmentVariable(PostgresDemoConfiguration.ConnectionEnvVar, original);
        }
    }

    private sealed class RecordingOutput : IOutput
    {
        public List<string> Messages { get; } = [];

        public void WriteLine(string message) => Messages.Add(message);
    }
}
