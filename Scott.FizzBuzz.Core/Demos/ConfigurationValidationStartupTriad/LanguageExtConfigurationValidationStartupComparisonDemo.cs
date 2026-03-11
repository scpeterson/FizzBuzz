using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConfigurationValidationStartupTriad;

public class LanguageExtConfigurationValidationStartupComparisonDemo : IDemo
{
    public const string DemoKey = "langext-startup-config-validation-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "configuration", "validation", "startup"];
    public string Description => "Applicative startup config validation with accumulated errors and pure result values.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ConfigurationValidationStartupRules.ExecuteLanguageExtPipeline(name, number)
            .Map(_ => unit);
}
