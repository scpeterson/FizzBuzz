using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public class ImperativeWriterMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeWriterMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeWriterMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "imperative-writer-monad-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "writer", "monad"];
    public string Description => "Imperative logging flow that manually threads both state and log accumulation.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!WriterMonadRules.TryParseStart(number, out var state, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!WriterMonadRules.TryResolveOps(name, out var ops, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var logs = new List<string>();
            foreach (var op in ops!)
            {
                var next = WriterMonadRules.Step(state, op);
                state = next.NextState;
                logs.Add(next.LogEntry);
            }

            _output.WriteLine($"Result: final state = {state}");
            logs.ForEach(_output.WriteLine);
        }, "Imperative Writer Monad Comparison");
}
