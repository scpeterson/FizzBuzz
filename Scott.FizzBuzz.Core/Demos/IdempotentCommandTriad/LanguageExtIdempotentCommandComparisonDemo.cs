using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public class LanguageExtIdempotentCommandComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtIdempotentCommandComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtIdempotentCommandComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-idempotent-command";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "idempotency", "triad"];
    public string Description => "LanguageExt idempotency flow using explicit immutable updates and Either-based command handling.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Idempotent Command Comparison",
            ComputeResult(name, number),
            (output, updated) =>
            {
                output.WriteLine("Result: command accepted.");
                output.WriteLine($"Processed commands: {string.Join(", ", updated.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))}");
            });

    private static Either<string, System.Collections.Generic.HashSet<string>> ComputeResult(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();
        var commandId = IdempotentCommandRules.NormalizeCommandId(name);
        var initial = new System.Collections.Generic.HashSet<string>(env.SeedProcessedCommandIds, StringComparer.OrdinalIgnoreCase);

        return
            from _ in LanguageExtIdempotentCommandRules.ParseAmount(number)
            from updated in LanguageExtIdempotentCommandRules.HandleLanguageExt(initial, commandId)
            select updated;
    }
}
