using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ParseValidateTriad;

public class LanguageExtEitherParseValidateDemo : IDemo
{
    public const string DemoKey = "langext-either-parse-validate";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "either", "validation", "parsing"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ValidateNotEmpty(number)
            .Bind(ParseInt)
            .Bind(RequirePositive)
            .Map(_ => unit);

    private static Either<string, string> ValidateNotEmpty(string? input) =>
        !string.IsNullOrWhiteSpace(input)
            ? Right<string, string>(input)
            : Left<string, string>("Number is required.");

    private static Either<string, int> ParseInt(string input) =>
        parseInt(input).ToEither("Not an integer.");

    private static Either<string, int> RequirePositive(int value) =>
        value > 0
            ? Right<string, int>(value)
            : Left<string, int>("Must be > 0.");
}
