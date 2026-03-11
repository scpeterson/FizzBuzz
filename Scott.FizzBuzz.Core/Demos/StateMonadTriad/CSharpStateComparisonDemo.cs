using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.StateMonadTriad;

public class CSharpStateComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpStateComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpStateComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-state-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "state", "monad"];
    public string Description => "Immutable C# state transitions still require explicit fold plumbing without State monad abstraction.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from plan in StateMonadRules.ResolvePlan(name)
                from step in StateMonadRules.ParseStep(number)
                select plan.Fold(new StateGame(0, 1, 0), (state, op) => StateMonadRules.Apply(op, step, state));

            result.Match(
                Right: state => _output.WriteLine($"Final state: score={state.Score}, multiplier={state.Multiplier}, penalties={state.Penalties}"),
                Left: error => _output.WriteLine($"Failed: {error}"));

            _output.WriteLine("C#/.NET comparison note: explicit fold and state passing are still required.");
        }, "C# State Comparison");
}
