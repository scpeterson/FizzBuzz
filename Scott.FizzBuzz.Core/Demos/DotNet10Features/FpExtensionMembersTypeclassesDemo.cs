using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.DotNet10Features;

public class FpExtensionMembersTypeclassesDemo : IDemo
{
    public const string DemoKey = "fp-extension-members-typeclasses";

    private readonly IOutput _output;

    public FpExtensionMembersTypeclassesDemo() : this(new ConsoleOutput())
    {
    }

    public FpExtensionMembersTypeclassesDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "dotnet10", "csharp14", "extensions", "either"];

    public Either<string, Unit> Run(string? name, string? number)
        => ExecuteWithSpacing(_output, () =>
        {
            var inputs = new[] { number ?? "12", "3", "abc" };

            foreach (var input in inputs)
            {
                ParsePositiveInt(input)
                    .MapFp(value => value * 2)
                    .BindFp(RequireMultipleOfFour)
                    .MapFp(value => $"Accepted value: {value}")
                    .Match(
                        Right: message => _output.WriteLine($"Input '{input}': {message}"),
                        Left: error => _output.WriteLine($"Input '{input}': {error}")
                    );
            }
        }, "C# 14 Extension Members with FP Composition");

    private static Either<string, int> ParsePositiveInt(string raw) =>
        int.TryParse(raw, out var parsed)
            ? parsed > 0
                ? Right<string, int>(parsed)
                : Left<string, int>("Value must be greater than zero.")
            : Left<string, int>($"Could not parse '{raw}' as an integer.");

    private static Either<string, int> RequireMultipleOfFour(int value) =>
        value % 4 == 0
            ? Right<string, int>(value)
            : Left<string, int>($"Value {value} is not a multiple of four.");
}
