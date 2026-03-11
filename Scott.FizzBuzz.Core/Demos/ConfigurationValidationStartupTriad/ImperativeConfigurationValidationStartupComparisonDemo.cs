using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public class ImperativeConfigurationValidationStartupComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeConfigurationValidationStartupComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeConfigurationValidationStartupComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-startup-config-validation-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "configuration", "validation", "startup"];
    public string Description => "Fail-fast startup validation using imperative conditionals and mutable branching.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ConfigurationValidationStartupRules.ExecuteImperative(name, number);

            result.Match(
                Right: config => _output.WriteLine(ConfigurationValidationStartupRules.FormatSummary(config)),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "Imperative Startup Configuration Validation Comparison");
}
