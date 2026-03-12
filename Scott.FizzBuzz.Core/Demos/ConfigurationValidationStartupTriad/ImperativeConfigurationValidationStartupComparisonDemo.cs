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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ConfigurationValidationStartupRules.ExecuteImperative(name, number);

            if (result.IsSuccess && result.Config is not null)
            {
                var config = result.Config;
                _output.WriteLine("Result: configuration valid.");
                _output.WriteLine(ConfigurationValidationStartupRules.FormatSummary(config));
            }
            else
            {
                _output.WriteLine($"Failed: {string.Join(" ", result.Errors)}");
            }
        }, "Imperative Startup Configuration Validation Comparison");
}
