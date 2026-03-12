namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public static class ConfigurationValidationStartupRules
{
    public sealed record StartupConfigInput(string Environment, string ConnectionString, int TimeoutSeconds, int MaxRetries, bool CacheEnabled);

    public sealed record StartupConfig(string Environment, string ConnectionString, int TimeoutSeconds, int MaxRetries, bool CacheEnabled);

    public static string NormalizeProfile(string? profile) =>
        string.IsNullOrWhiteSpace(profile) ? "dev" : profile.Trim().ToLowerInvariant();

    public static bool TryParseTimeoutSeconds(string? value, out int timeoutSeconds, out string? error)
    {
        if (int.TryParse(value, out timeoutSeconds))
        {
            error = null;
            return true;
        }

        error = "Timeout seconds must be an integer.";
        return false;
    }

    public static StartupConfigInput BuildCandidate(string profile, int timeoutSeconds) =>
        profile switch
        {
            "dev" => new StartupConfigInput("dev", "Host=localhost;Database=fizzbuzz_dev;", timeoutSeconds, MaxRetries: 2, CacheEnabled: true),
            "staging" => new StartupConfigInput("staging", "Host=staging-db;Database=fizzbuzz_stage;", timeoutSeconds, MaxRetries: 4, CacheEnabled: true),
            "prod" => new StartupConfigInput("prod", "Host=prod-db;Database=fizzbuzz_prod;", timeoutSeconds, MaxRetries: 6, CacheEnabled: false),
            "misconfigured" => new StartupConfigInput("misconfigured", "", timeoutSeconds, MaxRetries: 15, CacheEnabled: true),
            _ => new StartupConfigInput(profile, "", timeoutSeconds, MaxRetries: 2, CacheEnabled: true)
        };

    public static bool TryResolveCandidate(string? profile, string? timeoutValue, out StartupConfigInput? input, out string? error)
    {
        if (!TryParseTimeoutSeconds(timeoutValue, out var timeout, out error))
        {
            input = null;
            return false;
        }

        input = BuildCandidate(NormalizeProfile(profile), timeout);
        error = null;
        return true;
    }

    public static StartupValidationResult ExecuteImperative(string? profile, string? timeoutValue)
    {
        if (!TryResolveCandidate(profile, timeoutValue, out var input, out var error))
        {
            return StartupValidationResult.Fail(error ?? "Timeout seconds must be an integer.");
        }

        return ValidateImperative(input!);
    }

    public static StartupValidationResult ExecuteCSharpPipeline(string? profile, string? timeoutValue)
    {
        if (!TryResolveCandidate(profile, timeoutValue, out var input, out var error))
        {
            return StartupValidationResult.Fail(error ?? "Timeout seconds must be an integer.");
        }

        return ValidateCSharp(input!);
    }

    public static StartupValidationResult ValidateImperative(StartupConfigInput input)
    {
        if (!IsAllowedEnvironment(input.Environment))
        {
            return StartupValidationResult.Fail("Environment must be one of: dev|staging|prod.");
        }

        if (!IsConnectionStringValid(input.ConnectionString))
        {
            return StartupValidationResult.Fail("Connection string must include Host and Database segments.");
        }

        if (input.TimeoutSeconds is < 1 or > 120)
        {
            return StartupValidationResult.Fail("Timeout must be between 1 and 120 seconds.");
        }

        if (input.MaxRetries is < 0 or > 10)
        {
            return StartupValidationResult.Fail("Max retries must be between 0 and 10.");
        }

        if (input.Environment == "prod" && input.TimeoutSeconds > 30)
        {
            return StartupValidationResult.Fail("Prod timeout must be 30 seconds or less.");
        }

        if (input.Environment == "prod" && input.CacheEnabled)
        {
            return StartupValidationResult.Fail("Caching must be disabled in prod.");
        }

        return StartupValidationResult.Success(ToValidatedConfig(input));
    }

    public static StartupValidationResult ValidateCSharp(StartupConfigInput input)
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
            ? StartupValidationResult.Success(ToValidatedConfig(input))
            : StartupValidationResult.Fail(errors);
    }

    public static string FormatSummary(StartupConfig config) =>
        $"Environment={config.Environment}, Timeout={config.TimeoutSeconds}s, Retries={config.MaxRetries}, CacheEnabled={config.CacheEnabled}";

    private static StartupConfig ToValidatedConfig(StartupConfigInput input) =>
        new(input.Environment, input.ConnectionString, input.TimeoutSeconds, input.MaxRetries, input.CacheEnabled);

    internal static bool IsAllowedEnvironment(string environment) =>
        environment is "dev" or "staging" or "prod";

    internal static bool IsConnectionStringValid(string connectionString) =>
        !string.IsNullOrWhiteSpace(connectionString) &&
        connectionString.Contains("Host=", StringComparison.OrdinalIgnoreCase) &&
        connectionString.Contains("Database=", StringComparison.OrdinalIgnoreCase);

    public sealed record StartupValidationResult(ConfigurationValidationStartupRules.StartupConfig? Config, IReadOnlyList<string> Errors)
    {
        public bool IsSuccess => Errors.Count == 0 && Config is not null;

        public static StartupValidationResult Success(ConfigurationValidationStartupRules.StartupConfig config) => new(config, Array.Empty<string>());

        public static StartupValidationResult Fail(string error) => new(null, new[] { error });

        public static StartupValidationResult Fail(IReadOnlyList<string> errors) => new(null, errors);
    }
}
