using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public class CSharpConfigurationValidationStartupComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpConfigurationValidationStartupComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpConfigurationValidationStartupComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-startup-config-validation-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "configuration", "validation", "startup"];
    public string Description => "Composed C# startup validation pipeline that aggregates rule checks before app bootstrap.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ConfigurationValidationStartupRules.ExecuteCSharpPipeline(name, number);

            result.Match(
                Right: config => _output.WriteLine(ConfigurationValidationStartupRules.FormatSummary(config)),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Startup Configuration Validation Comparison");
}
