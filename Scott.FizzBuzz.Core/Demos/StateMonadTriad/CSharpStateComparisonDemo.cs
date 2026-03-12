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

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ResolvePlan(name)
                .Bind(plan => ParseStep(number).Map(step => plan.Aggregate(new StateGame(0, 1, 0), (state, op) => StateMonadRules.Apply(op, step, state))));

            if (result.IsSuccess)
            {
                var state = result.Value;
                _output.WriteLine($"Result: score={state.Score}, multiplier={state.Multiplier}, penalties={state.Penalties}");
            }
            else
            {
                _output.WriteLine($"Failed: {result.Error}");
            }

            _output.WriteLine("C#/.NET comparison note: explicit fold and state passing are still required.");
        }, "C# State Comparison");

    private static DemoResult<IReadOnlyList<string>> ResolvePlan(string? name) =>
        StateMonadRules.TryResolvePlan(name, out var plan, out var error)
            ? DemoResult<IReadOnlyList<string>>.Success(plan!)
            : DemoResult<IReadOnlyList<string>>.Failure(error);

    private static DemoResult<int> ParseStep(string? input) =>
        StateMonadRules.TryParseStep(input, out var step, out var error)
            ? DemoResult<int>.Success(step)
            : DemoResult<int>.Failure(error);

    private readonly record struct DemoResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DemoResult<T> Success(T value) => new(true, value, null);
        public static DemoResult<T> Failure(string? error) => new(false, default!, error);
        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess ? next(Value) : DemoResult<TNext>.Failure(Error);
        public DemoResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess ? DemoResult<TNext>.Success(map(Value)) : DemoResult<TNext>.Failure(Error);
    }
}
