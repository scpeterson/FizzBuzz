using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.IdempotentCommandTriad;

public class ImperativeIdempotentCommandComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeIdempotentCommandComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeIdempotentCommandComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-idempotent-command";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "idempotency", "triad"];
    public string Description => "Imperative idempotency flow that manually checks and mutates processed-command state.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!decimal.TryParse(number, out var amount) || amount < 0m)
            {
                _output.WriteLine("Failed: Amount must be a non-negative decimal.");
                return;
            }

            var env = new InMemoryFunctionalDemoEnvironment();
            var commandId = IdempotentCommandRules.NormalizeCommandId(name);
            var processed = new System.Collections.Generic.HashSet<string>(env.SeedProcessedCommandIds, StringComparer.OrdinalIgnoreCase);

            if (processed.Contains(commandId))
            {
                _output.WriteLine($"Duplicate ignored: {commandId}");
                return;
            }

            processed.Add(commandId);
            _output.WriteLine($"Processed command {commandId} for amount {amount:0.00}");
        }, "Imperative Idempotent Command Comparison");
}
