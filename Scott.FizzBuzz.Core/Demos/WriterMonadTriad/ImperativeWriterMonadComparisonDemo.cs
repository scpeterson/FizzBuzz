using LanguageExt;
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

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!int.TryParse(number, out var state))
            {
                _output.WriteLine("Failed: Start value must be numeric.");
                return;
            }

            var opsEither = WriterMonadRules.ResolveOps(name);
            if (opsEither.IsLeft)
            {
                _output.WriteLine($"Failed: {opsEither.LeftToList()[0]}");
                return;
            }

            var logs = new List<string>();
            foreach (var op in opsEither.RightToList()[0])
            {
                if (op >= 0)
                {
                    state += op;
                    logs.Add($"Added {op}, state={state}");
                }
                else
                {
                    var amt = Math.Abs(op);
                    state -= amt;
                    logs.Add($"Subtracted {amt}, state={state}");
                }
            }

            _output.WriteLine($"Final state: {state}");
            logs.ForEach(_output.WriteLine);
        }, "Imperative Writer Monad Comparison");
}
