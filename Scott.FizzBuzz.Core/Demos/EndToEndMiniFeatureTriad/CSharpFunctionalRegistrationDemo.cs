using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

public class CSharpFunctionalRegistrationDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpFunctionalRegistrationDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpFunctionalRegistrationDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-functional-registration";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "end-to-end"];
    public string Description => "C# fail-fast registration pipeline with Either composition.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            CSharpFunctionalRegistrationLogic.Register(name, number)
                .Match(
                    Right: user => _output.WriteLine($"Registered user {user.Id}"),
                    Left: error => _output.WriteLine(error));
        }, "C# Functional Registration");
}
