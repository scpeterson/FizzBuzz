using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.StateMonadTriad;

public class LanguageExtStateMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtStateMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtStateMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-state-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "state", "monad"];
    public string Description => "LanguageExt State monad composes transitions without explicit state plumbing.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt State Monad Comparison",
            ComputeResult(name, number),
            (output, state) =>
            {
                output.WriteLine($"Result: score={state.Score}, multiplier={state.Multiplier}, penalties={state.Penalties}");
                output.WriteLine("LanguageExt comparison note: state transitions are composed without explicit state plumbing.");
            });

    private static Either<string, StateGame> ComputeResult(string? name, string? number) =>
        from plan in LanguageExtStateMonadRules.ResolvePlan(name)
        from step in LanguageExtStateMonadRules.ParseStep(number)
        from _ in RunProgram(plan, step)
        select plan.Fold(new StateGame(0, 1, 0), (state, op) => StateMonadRules.Apply(op, step, state));

    private static Either<string, Unit> RunProgram(Seq<string> plan, int step)
    {
        var program = BuildProgram(plan, step);
        _ = program.Run(new StateGame(0, 1, 0));
        return Right<string, Unit>(Unit.Default);
    }

    private static State<StateGame, Unit> BuildProgram(Seq<string> plan, int step)
    {
        if (plan.IsEmpty)
        {
            return new State<StateGame, Unit>(state => new ValueTuple<Unit, StateGame?, bool>(unit, state, false));
        }

        var head = plan.Head;
        var tail = plan.Tail;

        return
            from _ in ApplyOperation(head, step)
            from __ in BuildProgram(tail, step)
            select unit;
    }

    private static State<StateGame, Unit> ApplyOperation(string operation, int step) =>
        new(state =>
        {
            var next = StateMonadRules.Apply(operation, step, state);
            return new ValueTuple<Unit, StateGame?, bool>(unit, next, false);
        });
}
