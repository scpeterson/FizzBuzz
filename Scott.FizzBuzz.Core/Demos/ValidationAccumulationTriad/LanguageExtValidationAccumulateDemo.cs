using LanguageExt;
using LanguageExt.Common;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ValidationAccumulationTriad;

public class LanguageExtValidationAccumulateDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtValidationAccumulateDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtValidationAccumulateDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-validation-accumulate";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "validation"];
    public string Description => "LanguageExt Validation accumulation that collects multiple independent rule failures.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Validation Accumulation",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, string> ComputeResult(string? name, string? number) =>
        ValidateName(name)
            .Bind(validName => ValidateAge(number).Map(validAge => (validName, validAge)))
            .ToEither()
            .MapLeft(errors => string.Join(" | ", errors.Map(error => error.Message)))
            .Map(_ => "validation passed.");

    private static Validation<Error, string> ValidateName(string? value) =>
        !string.IsNullOrWhiteSpace(value)
            ? Success<Error, string>(value)
            : Fail<Error, string>(Error.New("Name is required."));

    private static Validation<Error, int> ValidateAge(string? value) =>
        int.TryParse(value, out var parsed) && parsed >= 18
            ? Success<Error, int>(parsed)
            : Fail<Error, int>(Error.New("Age must be numeric and at least 18."));
}
