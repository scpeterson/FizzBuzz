using LanguageExt;
using Npgsql;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

public class ImperativePostgresDatabaseDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativePostgresDatabaseDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativePostgresDatabaseDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-db-postgres";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "database", "postgres", "io"];
    public string Description => "Imperative PostgreSQL workflow with inline mutation, SQL, and exception handling.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var connectionString = Environment.GetEnvironmentVariable(PostgresDemoConfiguration.ConnectionEnvVar);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _output.WriteLine($"Skipping: set {PostgresDemoConfiguration.ConnectionEnvVar} to run PostgreSQL demos.");
                return;
            }

            try
            {
                var parsedAge = int.Parse(number ?? "21");
                var parsedName = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var create = new NpgsqlCommand(
                    """
                    CREATE TABLE IF NOT EXISTS demo_people (
                        id          INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                        name        TEXT NOT NULL UNIQUE,
                        age         INT NOT NULL CHECK (age >= 0),
                        created_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
                    );
                    """,
                    connection);
                create.ExecuteNonQuery();

                using var command = new NpgsqlCommand(
                    """
                    INSERT INTO demo_people(name, age)
                    VALUES (@name, @age)
                    ON CONFLICT (name)
                    DO UPDATE SET age = EXCLUDED.age
                    RETURNING id, name, age;
                    """,
                    connection);

                command.Parameters.AddWithValue("name", parsedName);
                command.Parameters.AddWithValue("age", parsedAge);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                    _output.WriteLine($"Saved row: {reader.GetInt32(0)}|{reader.GetString(1)}|{reader.GetInt32(2)}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Database operation failed: {ex.Message}");
            }
        }, "Imperative PostgreSQL Database");
}
