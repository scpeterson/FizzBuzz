using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ParseValidateTriad;

public class CSharpParseValidatePipelineDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpParseValidatePipelineDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpParseValidatePipelineDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-parse-validate-pipeline";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "validation", "pipeline"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            ParsePositive(number ?? "12")
                .Match(
                    Right: value => _output.WriteLine($"Accepted: {value}"),
                    Left: error => _output.WriteLine(error));
        }, "C# Parse + Validate Pipeline");

    private static Either<string, int> ParsePositive(string raw) =>
        int.TryParse(raw, out var parsed)
            ? parsed > 0
                ? Right<string, int>(parsed)
                : Left<string, int>("Must be > 0.")
            : Left<string, int>("Not an integer.");
}
