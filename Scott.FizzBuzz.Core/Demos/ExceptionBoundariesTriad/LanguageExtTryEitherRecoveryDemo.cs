using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.Utilities;

namespace Scott.FizzBuzz.Core.Demos.ExceptionBoundariesTriad;

public class LanguageExtTryEitherRecoveryDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtTryEitherRecoveryDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtTryEitherRecoveryDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-try-either-recovery";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "exceptions", "either", "try"];
    public string Description => "LanguageExt Try/Either boundary that turns exception-prone work into explicit recoverable values.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Try/Either Recovery",
            DivideByUserInput(number ?? "2"),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, int> DivideByUserInput(string raw) =>
        TryEither(
                () => int.Parse(raw),
                ex => LanguageExt.Common.Error.New($"Input parse failure: {ex.Message}"))
            .MapLeft(error => error.Message)
            .Bind(denominator => denominator == 0
                ? Left<string, int>("Cannot divide by zero.")
                : Right<string, int>(100 / denominator));
}
