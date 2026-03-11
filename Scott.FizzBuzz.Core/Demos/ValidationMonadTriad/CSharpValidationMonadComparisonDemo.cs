using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;

public class CSharpValidationMonadComparisonDemo : IDemo
{
    public const string DemoKey = "csharp-validation-monad-comparison";
    public const string NameRequiredMessage = "Name is required.";
    public const string NameMinLengthMessage = "Name must be at least 3 characters.";
    public const string NameLettersOnlyMessage = "Name must contain letters only.";
    public const string AgeNumericMessage = "Age must be numeric.";
    public const string AgeRangeMessage = "Age must be between 18 and 120.";
    public const string ErrorAccumulationNote = "C#/.NET comparison note: explicit List<string> accumulation is required.";
    public const string SuccessAccumulationNote = "C#/.NET comparison note: custom accumulation logic still required to gather all errors.";

    private readonly IOutput _output;

    public CSharpValidationMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpValidationMonadComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "validation", "monad"];
    public string Description => "Manual error-list accumulation in C# to emulate Validation-style accumulation.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var errors = new List<string>();
            var normalizedName = name?.Trim() ?? string.Empty;

            if (normalizedName.Length == 0)
            {
                errors.Add(NameRequiredMessage);
            }

            if (normalizedName.Length < 3)
            {
                errors.Add(NameMinLengthMessage);
            }

            if (normalizedName.Any(ch => !char.IsLetter(ch)))
            {
                errors.Add(NameLettersOnlyMessage);
            }

            if (!int.TryParse(number, out var age))
            {
                errors.Add(AgeNumericMessage);
            }
            else if (age is < 18 or > 120)
            {
                errors.Add(AgeRangeMessage);
            }

            if (errors.Count > 0)
            {
                _output.WriteLine(string.Join(" | ", errors));
                _output.WriteLine(ErrorAccumulationNote);
                return;
            }

            _output.WriteLine($"Validated candidate: {normalizedName} ({age})");
            _output.WriteLine(SuccessAccumulationNote);
        }, "C# Validation Monad Comparison");
}
