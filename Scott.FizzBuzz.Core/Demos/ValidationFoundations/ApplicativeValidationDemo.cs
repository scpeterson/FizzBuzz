using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;
using ApplicativeValidation = Scott.FizzBuzz.Core.ApplicativeValidationExample.ApplicativeValidationDemo;

namespace Scott.FizzBuzz.Core.Demos.ValidationFoundations;

public class ApplicativeValidationDemo : IDemo
{
    private readonly IOutput _output;

    public ApplicativeValidationDemo() : this(new ConsoleOutput())
    {
    }

    public ApplicativeValidationDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "applicative-validation";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "validation", "applicative"];
    public string Description => "Applicative validation with reusable Validation functions and accumulated errors.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ApplicativeValidation.ValidateToEntity(name, number)
            .Match(
                Succ: entity =>
                    ExecuteWithSpacing(
                        _output,
                        () => _output.WriteLine($"Validated entity: firstName={entity.FirstName}, age={entity.Age}"),
                        "Applicative Validation"),
                Fail: errors => Left<string, Unit>(string.Join(" | ", errors.Map(error => error.Message))));
}
