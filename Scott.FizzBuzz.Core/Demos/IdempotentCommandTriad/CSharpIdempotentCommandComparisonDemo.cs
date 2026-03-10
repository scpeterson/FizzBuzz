using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public class CSharpIdempotentCommandComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpIdempotentCommandComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpIdempotentCommandComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => "csharp-idempotent-command";
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "idempotency", "triad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var env = new InMemoryFunctionalDemoEnvironment();
            var commandId = IdempotentCommandRules.NormalizeCommandId(name);

            var result =
                from amount in IdempotentCommandRules.ParseAmount(number)
                let handled = IdempotentCommandRules.HandleCSharp(env.SeedProcessedCommandIds, commandId)
                select (amount, handled.IsDuplicate);

            result.Match(
                Right: tuple =>
                {
                    var outcome = tuple.IsDuplicate ? "Duplicate ignored" : "Processed";
                    _output.WriteLine($"{outcome}: {commandId} ({tuple.amount:0.00})");
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Idempotent Command Comparison");
}
