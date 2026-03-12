using Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

namespace Scott.FizzBuzz.Core.Tests.Demos.DatabasePostgresTriad;

internal static class PostgresTestEnvironment
{
    private static readonly object Sync = new();

    public static void WithTempConnectionString(string? value, Action action)
    {
        lock (Sync)
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
    }
}
