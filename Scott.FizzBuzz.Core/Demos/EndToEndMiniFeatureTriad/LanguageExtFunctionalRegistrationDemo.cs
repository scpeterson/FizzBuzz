using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

public class LanguageExtFunctionalRegistrationDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtFunctionalRegistrationDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtFunctionalRegistrationDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-functional-registration";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "end-to-end"];
    public string Description => "LanguageExt registration flow with Validation + Either boundaries.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt End-to-End Registration",
            LanguageExtFunctionalRegistrationLogic.Register(name, number),
            (output, user) =>
            {
                output.WriteLine("Result: registration succeeded.");
                output.WriteLine($"User: id={user.Id}, name={user.Name}, age={user.Age}");
            });
}
