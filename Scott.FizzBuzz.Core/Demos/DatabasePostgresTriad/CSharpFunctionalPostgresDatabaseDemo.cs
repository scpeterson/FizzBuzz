using Npgsql;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

public class CSharpFunctionalPostgresDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpFunctionalPostgresDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpFunctionalPostgresDatabaseDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-db-postgres";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "database", "postgres", "io"];
    public string Description => "C# functional PostgreSQL pipeline with plain result types and explicit IO boundaries.";

    public DemoExecutionResult Run(string? name, string? number)
    {
        var result = GetConnectionString()
            .Bind(connectionString => ParseInput(name, number)
                .Bind(input => EnsureSchema(connectionString)
                    .Bind(_ => UpsertByName(connectionString, input))));

        if (!result.IsSuccess)
            return DemoExecutionResult.Failure(result.Error ?? "Database operation failed.");

        return ExecuteWithSpacing(
            _output,
            () => _output.WriteLine($"Saved row: {result.Value.Id}|{result.Value.Name}|{result.Value.Age}"),
            "C# Functional PostgreSQL Database");
    }

    private static DatabaseResult<string> GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable(PostgresDemoConfiguration.ConnectionEnvVar);
        return !string.IsNullOrWhiteSpace(connectionString)
            ? DatabaseResult<string>.Success(connectionString.Trim())
            : DatabaseResult<string>.Failure($"Set {PostgresDemoConfiguration.ConnectionEnvVar} to a valid PostgreSQL connection string.");
    }

    private static DatabaseResult<PostgresDatabaseInput> ParseInput(string? name, string? number)
    {
        var sanitizedName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();

        if (!int.TryParse(number ?? "21", out var age))
            return DatabaseResult<PostgresDatabaseInput>.Failure("Age must be an integer.");

        if (age < 0)
            return DatabaseResult<PostgresDatabaseInput>.Failure("Age must be non-negative.");

        return DatabaseResult<PostgresDatabaseInput>.Success(new PostgresDatabaseInput(sanitizedName, age));
    }

    private static DatabaseResult<Unit> EnsureSchema(string connectionString)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(
                """
                CREATE TABLE IF NOT EXISTS demo_people (
                    id          INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                    name        TEXT NOT NULL UNIQUE,
                    age         INT NOT NULL CHECK (age >= 0),
                    created_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
                );
                """,
                connection);

            command.ExecuteNonQuery();
            return DatabaseResult<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return DatabaseResult<Unit>.Failure($"Schema setup failed: {ex.Message}");
        }
    }

    private static DatabaseResult<PostgresPersonRecord> UpsertByName(string connectionString, PostgresDatabaseInput input)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(
                """
                INSERT INTO demo_people(name, age)
                VALUES (@name, @age)
                ON CONFLICT (name)
                DO UPDATE SET age = EXCLUDED.age
                RETURNING id, name, age;
                """,
                connection);

            command.Parameters.AddWithValue("name", input.Name);
            command.Parameters.AddWithValue("age", input.Age);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return DatabaseResult<PostgresPersonRecord>.Failure("No row returned from upsert.");

            return DatabaseResult<PostgresPersonRecord>.Success(
                new PostgresPersonRecord(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2)));
        }
        catch (Exception ex)
        {
            return DatabaseResult<PostgresPersonRecord>.Failure($"Database upsert failed: {ex.Message}");
        }
    }

    private readonly record struct DatabaseResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DatabaseResult<T> Success(T value) => new(true, value, null);
        public static DatabaseResult<T> Failure(string error) => new(false, default!, error);

        public DatabaseResult<TNext> Bind<TNext>(Func<T, DatabaseResult<TNext>> next) =>
            IsSuccess
                ? next(Value)
                : DatabaseResult<TNext>.Failure(Error ?? "Database operation failed.");
    }

    private readonly record struct Unit
    {
        public static Unit Value => new();
    }
}
