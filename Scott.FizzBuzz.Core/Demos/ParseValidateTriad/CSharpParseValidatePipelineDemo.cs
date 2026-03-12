using Scott.FizzBuzz.Core.Interfaces;
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
    public string Description => "Plain C# pipeline: validate, parse, and enforce business rules through explicit step composition.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            // Each step returns the same local result type, so the pipeline stays
            // explicit without introducing a third-party monad into the C# variant.
            var result = ValidateNotEmpty(number)
                .Bind(ParseInt)
                .Bind(RequirePositive);

            _output.WriteLine(result.IsSuccess
                ? $"Result: accepted = {result.Value}"
                : $"Failed: {result.Error}");
        }, "C# Parse + Validate Pipeline");

    private static ParseResult<string> ValidateNotEmpty(string? raw) =>
        !string.IsNullOrWhiteSpace(raw)
            ? ParseResult<string>.Success(raw)
            : ParseResult<string>.Failure("Number is required.");

    private static ParseResult<int> ParseInt(string raw) =>
        int.TryParse(raw, out var parsed)
            ? ParseResult<int>.Success(parsed)
            : ParseResult<int>.Failure("Not an integer.");

    private static ParseResult<int> RequirePositive(int value) =>
        value > 0
            ? ParseResult<int>.Success(value)
            : ParseResult<int>.Failure("Must be > 0.");

    private readonly record struct ParseResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static ParseResult<T> Success(T value) => new(true, value, null);
        public static ParseResult<T> Failure(string error) => new(false, default!, error);

        public ParseResult<TNext> Bind<TNext>(Func<T, ParseResult<TNext>> next) =>
            IsSuccess
                ? next(Value)
                : ParseResult<TNext>.Failure(Error ?? "Parse/validate pipeline failed.");
    }
}
