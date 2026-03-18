using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FunctionalProgrammingTriads.Core.Demos.DatabasePostgresTriad;

public static class PostgresDemoConfiguration
{
    public const string ConnectionEnvVar = "FUNCTIONAL_PROGRAMMING_TRIADS_POSTGRES_CONNECTION";
    public const string LegacyConnectionEnvVar = "FIZZBUZZ_POSTGRES_CONNECTION";

    public static Either<string, string> GetConnectionString() =>
        Optional(ReadConnectionString())
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToEither($"Set {ConnectionEnvVar} to a valid PostgreSQL connection string.")
            .Map(value => value.Trim());

    private static string? ReadConnectionString() =>
        Environment.GetEnvironmentVariable(ConnectionEnvVar)
        ?? Environment.GetEnvironmentVariable(LegacyConnectionEnvVar);
}
