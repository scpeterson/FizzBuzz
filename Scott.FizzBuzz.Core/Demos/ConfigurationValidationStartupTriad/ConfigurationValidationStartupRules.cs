using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public static class ConfigurationValidationStartupRules
{
    public sealed record StartupConfigInput(string Environment, string ConnectionString, int TimeoutSeconds, int MaxRetries, bool CacheEnabled);

    public sealed record StartupConfig(string Environment, string ConnectionString, int TimeoutSeconds, int MaxRetries, bool CacheEnabled);

    public static string NormalizeProfile(string? profile) =>
        string.IsNullOrWhiteSpace(profile) ? "dev" : profile.Trim().ToLowerInvariant();

    public static Either<string, int> ParseTimeoutSeconds(string? value) =>
        int.TryParse(value, out var parsed)
            ? Right<string, int>(parsed)
            : Left<string, int>("Timeout seconds must be an integer.");

    public static StartupConfigInput BuildCandidate(string profile, int timeoutSeconds) =>
        profile switch
        {
            "dev" => new StartupConfigInput("dev", "Host=localhost;Database=fizzbuzz_dev;", timeoutSeconds, MaxRetries: 2, CacheEnabled: true),
            "staging" => new StartupConfigInput("staging", "Host=staging-db;Database=fizzbuzz_stage;", timeoutSeconds, MaxRetries: 4, CacheEnabled: true),
            "prod" => new StartupConfigInput("prod", "Host=prod-db;Database=fizzbuzz_prod;", timeoutSeconds, MaxRetries: 6, CacheEnabled: false),
            "misconfigured" => new StartupConfigInput("misconfigured", "", timeoutSeconds, MaxRetries: 15, CacheEnabled: true),
            _ => new StartupConfigInput(profile, "", timeoutSeconds, MaxRetries: 2, CacheEnabled: true)
        };

    public static Either<string, StartupConfigInput> ResolveCandidate(string? profile, string? timeoutValue) =>
        ParseTimeoutSeconds(timeoutValue)
            .Map(timeout => BuildCandidate(NormalizeProfile(profile), timeout));

    public static Either<string, StartupConfig> ExecuteImperative(string? profile, string? timeoutValue) =>
        ResolveCandidate(profile, timeoutValue)
            .Bind(ValidateImperative);

    public static Either<string, StartupConfig> ExecuteCSharpPipeline(string? profile, string? timeoutValue) =>
        ResolveCandidate(profile, timeoutValue)
            .Bind(ValidateCSharp);

    public static Either<string, StartupConfig> ExecuteLanguageExtPipeline(string? profile, string? timeoutValue) =>
        ResolveCandidate(profile, timeoutValue)
            .Bind(input =>
                ValidateLanguageExt(input)
                    .ToEither()
                    .MapLeft(errors => string.Join(" ", errors)));

    public static Either<string, StartupConfig> ValidateImperative(StartupConfigInput input)
    {
        if (!IsAllowedEnvironment(input.Environment))
        {
            return Left<string, StartupConfig>("Environment must be one of: dev|staging|prod.");
        }

        if (!IsConnectionStringValid(input.ConnectionString))
        {
            return Left<string, StartupConfig>("Connection string must include Host and Database segments.");
        }

        if (input.TimeoutSeconds is < 1 or > 120)
        {
            return Left<string, StartupConfig>("Timeout must be between 1 and 120 seconds.");
        }

        if (input.MaxRetries is < 0 or > 10)
        {
            return Left<string, StartupConfig>("Max retries must be between 0 and 10.");
        }

        if (input.Environment == "prod" && input.TimeoutSeconds > 30)
        {
            return Left<string, StartupConfig>("Prod timeout must be 30 seconds or less.");
        }

        if (input.Environment == "prod" && input.CacheEnabled)
        {
            return Left<string, StartupConfig>("Caching must be disabled in prod.");
        }

        return Right<string, StartupConfig>(ToValidatedConfig(input));
    }

    public static Either<string, StartupConfig> ValidateCSharp(StartupConfigInput input)
    {
        var errors = new List<string>();

        if (!IsAllowedEnvironment(input.Environment))
        {
            errors.Add("Environment must be one of: dev|staging|prod.");
        }

        if (!IsConnectionStringValid(input.ConnectionString))
        {
            errors.Add("Connection string must include Host and Database segments.");
        }

        if (input.TimeoutSeconds is < 1 or > 120)
        {
            errors.Add("Timeout must be between 1 and 120 seconds.");
        }

        if (input.MaxRetries is < 0 or > 10)
        {
            errors.Add("Max retries must be between 0 and 10.");
        }

        if (input.Environment == "prod" && input.TimeoutSeconds > 30)
        {
            errors.Add("Prod timeout must be 30 seconds or less.");
        }

        if (input.Environment == "prod" && input.CacheEnabled)
        {
            errors.Add("Caching must be disabled in prod.");
        }

        return errors.Count == 0
            ? Right<string, StartupConfig>(ToValidatedConfig(input))
            : Left<string, StartupConfig>(string.Join(" ", errors));
    }

    public static Validation<Seq<string>, StartupConfig> ValidateLanguageExt(StartupConfigInput input) =>
        (
            ValidateEnvironment(input.Environment),
            ValidateConnectionString(input.ConnectionString),
            ValidateTimeout(input.TimeoutSeconds),
            ValidateMaxRetries(input.MaxRetries),
            ValidateProdTimeout(input.Environment, input.TimeoutSeconds),
            ValidateProdCaching(input.Environment, input.CacheEnabled)
        )
        .Apply((environment, connectionString, timeout, retries, _, _) =>
            new StartupConfig(environment, connectionString, timeout, retries, input.CacheEnabled));

    public static string FormatSummary(StartupConfig config) =>
        $"Environment={config.Environment}, Timeout={config.TimeoutSeconds}s, Retries={config.MaxRetries}, CacheEnabled={config.CacheEnabled}";

    private static StartupConfig ToValidatedConfig(StartupConfigInput input) =>
        new(input.Environment, input.ConnectionString, input.TimeoutSeconds, input.MaxRetries, input.CacheEnabled);

    private static bool IsAllowedEnvironment(string environment) =>
        environment is "dev" or "staging" or "prod";

    private static bool IsConnectionStringValid(string connectionString) =>
        !string.IsNullOrWhiteSpace(connectionString) &&
        connectionString.Contains("Host=", StringComparison.OrdinalIgnoreCase) &&
        connectionString.Contains("Database=", StringComparison.OrdinalIgnoreCase);

    private static Validation<Seq<string>, string> ValidateEnvironment(string environment) =>
        IsAllowedEnvironment(environment)
            ? Success<Seq<string>, string>(environment)
            : Fail<Seq<string>, string>(Seq1("Environment must be one of: dev|staging|prod."));

    private static Validation<Seq<string>, string> ValidateConnectionString(string connectionString) =>
        IsConnectionStringValid(connectionString)
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
