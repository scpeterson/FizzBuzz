using LanguageExt;
using LanguageExt.Common;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static Scott.FizzBuzz.Core.Utilities;

namespace Scott.FizzBuzz.Core.Demos.DotNet10Features;

public class FpJsonStrictValidationDemo : IDemo
{
    public const string DemoKey = "fp-json-strict-validation";

    private readonly IOutput _output;

    public FpJsonStrictValidationDemo() : this(new ConsoleOutput())
    {
    }

    public FpJsonStrictValidationDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "dotnet10", "json", "validation", "strict"];
    public string Description => "Advanced .NET 10 demo combining strict JSON parsing with functional validation and explicit error reporting.";

    public DemoExecutionResult Run(string? name, string? number)
        => ExecuteWithSpacing(_output, () =>
        {
            var scenarios = new[]
            {
                ("Valid payload", """{"name":"Ada","age":37}"""),
                ("Unknown field", """{"name":"Ada","age":37,"role":"admin"}"""),
                ("Duplicate property", """{"name":"Ada","name":"Grace","age":37}"""),
                ("Domain validation failure", """{"name":"Ada","age":12}""")
            };

            foreach (var (label, json) in scenarios)
            {
                _output.WriteLine($"Scenario: {label}");
                FpJsonStrictValidationLogic.ParseRequestStrict(json)
                    .Match(
                        Right: request =>
                            FpJsonStrictValidationLogic.ValidateAdult(request).Match(
                                Succ: person => _output.WriteLine($"  Right: {person.Name} ({person.Age})"),
                                Fail: (Seq<Error> errors) => _output.WriteLine($"  Left: {errors.RenderMessages()}")),
                        Left: error => _output.WriteLine($"  Left: {error.Message}"));
            }
        }, ".NET 10 Strict JSON -> Functional Validation");
}
