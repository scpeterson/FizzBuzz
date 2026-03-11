using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ExceptionBoundariesTriad;

public class CSharpResultRecoveryDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpResultRecoveryDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpResultRecoveryDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-result-recovery";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "exceptions"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = DivideByUserInput(number ?? "2");
            _output.WriteLine(result.IsSuccess ? $"Result: {result.Value}" : result.Error);
        }, "C# Result Recovery");

    private static CalcResult DivideByUserInput(string raw)
    {
        if (!int.TryParse(raw, out var denominator))
            return CalcResult.Fail("Input must be an integer.");
        if (denominator == 0)
            return CalcResult.Fail("Cannot divide by zero.");

        return CalcResult.Ok(100 / denominator);
    }

    private readonly record struct CalcResult(bool IsSuccess, int Value, string Error)
    {
        public static CalcResult Ok(int value) => new(true, value, string.Empty);
        public static CalcResult Fail(string error) => new(false, 0, error);
    }
}
