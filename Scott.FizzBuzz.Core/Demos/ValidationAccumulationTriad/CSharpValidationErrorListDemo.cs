using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var errors = Validate(name, number);
            _output.WriteLine(errors.Count == 0
                ? "Validation passed."
                : string.Join(" | ", errors));
        }, "C# Validation (Error List)");

    private static List<string> Validate(string? name, string? number)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");

        if (!int.TryParse(number, out var age))
            errors.Add("Age must be numeric.");
        else if (age < 18)
            errors.Add("Age must be at least 18.");

        return errors;
    }
}
