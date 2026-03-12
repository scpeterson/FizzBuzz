using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.NullOptionTriad;

public class LanguageExtOptionPipelineDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtOptionPipelineDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtOptionPipelineDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-option-pipeline";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "option", "null"];
    public string Description => "LanguageExt Option pipeline for null-sensitive normalization and validation without nested checks.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Option Pipeline",
            ComputeResult(name),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, string> ComputeResult(string? name) =>
        Optional(name)
            .Map(value => value.Trim())
            .Filter(value => value.Length > 0)
            .ToEither("Name missing or empty.")
            .Map(value => $"validated name = {value}");
}
