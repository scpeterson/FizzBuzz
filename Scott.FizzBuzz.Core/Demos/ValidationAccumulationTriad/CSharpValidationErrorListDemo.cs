using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ValidationAccumulationTriad;

public class CSharpValidationErrorListDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpValidationErrorListDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpValidationErrorListDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-validation-error-list";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "validation"];
    public string Description => "Plain C# validation accumulation built from small validators that each contribute zero or more errors.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            // Unlike the imperative first-error example, this version lets each
            // validator contribute its own errors before we render the summary.
            var errors = Validate(name, number);
            _output.WriteLine(errors.Count == 0
                ? "Result: validation passed."
                : $"Failed: {string.Join(" | ", errors)}");
        }, "C# Validation (Error List)");

    private static IReadOnlyList<string> Validate(string? name, string? number) =>
        ValidateName(name)
            .Concat(ValidateAge(number))
            .ToArray();

    private static IEnumerable<string> ValidateName(string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? ["Name is required."]
            : [];

    private static IEnumerable<string> ValidateAge(string? number)
    {
        if (!int.TryParse(number, out var age))
            return ["Age must be numeric."];

        return age < 18
            ? ["Age must be at least 18."]
            : [];
    }
}
