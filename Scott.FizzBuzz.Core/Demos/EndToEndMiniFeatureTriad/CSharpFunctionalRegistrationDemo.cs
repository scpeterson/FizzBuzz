using Scott.FizzBuzz.Core.Interfaces;
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
    public string Description => "C# fail-fast registration pipeline with a plain C# result type.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = CSharpFunctionalRegistrationLogic.Register(name, number);
            _output.WriteLine(result.IsSuccess
                ? $"Result: registered user {result.Value!.Id}"
                : $"Failed: {result.Error}");
        }, "C# Functional Registration");
}
