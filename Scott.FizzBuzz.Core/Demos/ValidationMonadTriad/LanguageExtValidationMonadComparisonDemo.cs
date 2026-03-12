using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;

public class LanguageExtValidationMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtValidationMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtValidationMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-validation-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "validation", "monad"];
    public string Description => "LanguageExt Validation applicative composition with error accumulation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Validation Monad Comparison",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: validated candidate = {result.Name}, age {result.Age}"));

    private static Either<string, ValidationMonadCandidate> ComputeResult(string? name, string? number) =>
        ValidationMonadRules.ValidateCandidate(name, number)
            .ToEither()
            .MapLeft(error => error.ToString());
}
