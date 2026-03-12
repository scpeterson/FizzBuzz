using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.StateMonadTriad;

public class ImperativeStateComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeStateComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeStateComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-state-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "state", "monad"];
    public string Description => "Mutable state updated across imperative steps with explicit reassignment and branching.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!StateMonadRules.TryResolvePlan(name, out var plan, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!StateMonadRules.TryParseStep(number, out var step, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var score = 0;
            var multiplier = 1;
            var penalties = 0;

            foreach (var op in plan!)
            {
                if (op == "add")
                {
                    score += step * multiplier;
                }
                else if (op == "boost")
                {
                    multiplier += 1;
                }
                else if (op == "penalty")
                {
                    score -= 3;
                    penalties += 1;
                }
            }

            _output.WriteLine($"Final state: score={score}, multiplier={multiplier}, penalties={penalties}");
            _output.WriteLine("Imperative comparison note: all state fields are threaded manually and mutated step by step.");
        }, "Imperative State Comparison");
}
