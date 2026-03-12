using LanguageExt;
using Scott.FizzBuzz.Core.MonadicFunctions;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.MonadFoundations;

public class MonadicFunctionsDemo : IDemo
{
    private readonly IOutput _output;

    public MonadicFunctionsDemo() : this(new ConsoleOutput())
    {
    }

    public MonadicFunctionsDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "monadic-functions";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "monads", "option", "either", "comparison", "beginner"];
    public string Description => "Why monads help: hidden state and exceptions versus Option/Either composition.";
    public DemoExecutionResult Run(string? _1, string? _2) =>
        ExecuteWithSpacing(_output, () =>
        {
            _output.WriteLine("=== Imperative: hidden state ===");
            ImpureFunctions.SetSuffix("xyz");
            var first = ImpureFunctions.ClosureLength("cat");
            ImpureFunctions.SetSuffix("abcdefgh");
            var second = ImpureFunctions.ClosureLength("cat");
            _output.WriteLine($"Same input 'cat' -> {first}, then {second} after hidden state changed.");

            _output.WriteLine(string.Empty);
            _output.WriteLine("=== Imperative: exception boundary ===");
            try
            {
                var unsafeResult = ImpureFunctions.Divide(10m, 0m);
                _output.WriteLine($"10 / 0 = {unsafeResult}");
            }
            catch (DivideByZeroException ex)
            {
                _output.WriteLine($"Exception: {ex.Message}");
            }

            _output.WriteLine(string.Empty);
            _output.WriteLine("=== Monadic Option: explicit failure, no exception ===");
            var safeDivision = PureFunctions.SafeDivide(10m, 0m)
                .Map(PureFunctions.Twice);
            safeDivision.Match(
                Some: value => _output.WriteLine($"Twice(10 / 0) = {value}"),
                None: () => _output.WriteLine("No value because denominator was zero."));

            _output.WriteLine(string.Empty);
            _output.WriteLine("=== Monadic Either: fail-fast with message ===");
            ParseInt("15")
                .Bind(RequireLessThan(10))
                .Map(PureFunctions.Twice)
                .Match(
                    Right: value => _output.WriteLine($"Valid transformed value: {value}"),
                    Left: error => _output.WriteLine($"Validation error: {error}"));
        },
        "Monadic Functions Demo");

    private static Either<string, int> ParseInt(string input) =>
        parseInt(input).ToEither("Not a valid integer.");

    private static Func<int, Either<string, int>> RequireLessThan(int limit) =>
        value => value < limit
            ? Right<string, int>(value)
            : Left<string, int>($"Must be less than {limit}.");
}
