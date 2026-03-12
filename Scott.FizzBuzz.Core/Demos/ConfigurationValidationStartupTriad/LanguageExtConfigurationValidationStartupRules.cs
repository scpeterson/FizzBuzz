using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public static class LanguageExtConfigurationValidationStartupRules
{
    public static Either<string, ConfigurationValidationStartupRules.StartupConfig> ExecuteLanguageExtPipeline(string? profile, string? timeoutValue)
    {
        if (!ConfigurationValidationStartupRules.TryResolveCandidate(profile, timeoutValue, out var input, out var error))
        {
            return Left<string, ConfigurationValidationStartupRules.StartupConfig>(error ?? "Timeout seconds must be an integer.");
        }

        return ValidateLanguageExt(input!)
            .ToEither()
            .MapLeft(errors => string.Join(" ", errors));
    }

    public static Validation<Seq<string>, ConfigurationValidationStartupRules.StartupConfig> ValidateLanguageExt(
        ConfigurationValidationStartupRules.StartupConfigInput input) =>
        (
            ValidateEnvironment(input.Environment),
            ValidateConnectionString(input.ConnectionString),
            ValidateTimeout(input.TimeoutSeconds),
            ValidateMaxRetries(input.MaxRetries),
            ValidateProdTimeout(input.Environment, input.TimeoutSeconds),
            ValidateProdCaching(input.Environment, input.CacheEnabled)
        )
        .Apply((environment, connectionString, timeout, retries, _, _) =>
            new ConfigurationValidationStartupRules.StartupConfig(
                environment,
                connectionString,
                timeout,
                retries,
                input.CacheEnabled));

    private static Validation<Seq<string>, string> ValidateEnvironment(string environment) =>
        ConfigurationValidationStartupRules.IsAllowedEnvironment(environment)
            ? Success<Seq<string>, string>(environment)
            : Fail<Seq<string>, string>(Seq1("Environment must be one of: dev|staging|prod."));

    private static Validation<Seq<string>, string> ValidateConnectionString(string connectionString) =>
        ConfigurationValidationStartupRules.IsConnectionStringValid(connectionString)
            ? Success<Seq<string>, string>(connectionString)
            : Fail<Seq<string>, string>(Seq1("Connection string must include Host and Database segments."));

    private static Validation<Seq<string>, int> ValidateTimeout(int timeoutSeconds) =>
        timeoutSeconds is >= 1 and <= 120
            ? Success<Seq<string>, int>(timeoutSeconds)
            : Fail<Seq<string>, int>(Seq1("Timeout must be between 1 and 120 seconds."));

    private static Validation<Seq<string>, int> ValidateMaxRetries(int maxRetries) =>
        maxRetries is >= 0 and <= 10
            ? Success<Seq<string>, int>(maxRetries)
            : Fail<Seq<string>, int>(Seq1("Max retries must be between 0 and 10."));

    private static Validation<Seq<string>, Unit> ValidateProdTimeout(string environment, int timeoutSeconds) =>
        environment == "prod" && timeoutSeconds > 30
            ? Fail<Seq<string>, Unit>(Seq1("Prod timeout must be 30 seconds or less."))
            : Success<Seq<string>, Unit>(unit);

    private static Validation<Seq<string>, Unit> ValidateProdCaching(string environment, bool cacheEnabled) =>
        environment == "prod" && cacheEnabled
            ? Fail<Seq<string>, Unit>(Seq1("Caching must be disabled in prod."))
            : Success<Seq<string>, Unit>(unit);
}
