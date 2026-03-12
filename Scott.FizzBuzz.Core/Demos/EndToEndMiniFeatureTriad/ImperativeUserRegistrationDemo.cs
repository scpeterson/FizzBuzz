using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

public class ImperativeUserRegistrationDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeUserRegistrationDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeUserRegistrationDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-user-registration";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "end-to-end"];
    public string Description => "Imperative registration workflow with early exits for validation and inline record creation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var rawName = name ?? "";
            var rawAge = number ?? "";

            if (string.IsNullOrWhiteSpace(rawName))
            {
                _output.WriteLine("Name is required.");
                return;
            }

            if (!int.TryParse(rawAge, out var age))
            {
                _output.WriteLine("Age must be numeric.");
                return;
            }

            if (age < 18)
            {
                _output.WriteLine("Must be at least 18.");
                return;
            }

            var id = $"{rawName.Trim().ToLowerInvariant()}-{age}";
            _output.WriteLine($"Result: registered user {id}");
        }, "Imperative User Registration");
}
