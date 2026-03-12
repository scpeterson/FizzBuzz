using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ValidationAccumulationTriad;

public class ImperativeValidationFirstErrorDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeValidationFirstErrorDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeValidationFirstErrorDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-validation-first-error";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "validation"];
    public string Description => "Imperative validation flow that stops at the first failing rule.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ValidateImperative(name, number);
            _output.WriteLine(result);
        }, "Imperative Validation (First Error)");

    private static string ValidateImperative(string? name, string? number)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name is required.";
        if (!int.TryParse(number, out var age))
            return "Age must be numeric.";
        if (age < 18)
            return "Age must be at least 18.";
        return "Validation passed.";
    }
}
