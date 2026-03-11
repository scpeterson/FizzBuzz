using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public class CSharpWriterMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpWriterMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public CSharpWriterMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "csharp-writer-monad-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "writer", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result =
                from start in WriterMonadRules.ParseStart(number)
                from ops in WriterMonadRules.ResolveOps(name)
                select (start, ops);

            result.Match(
                Right: tuple =>
                {
                    var logs = new List<string>();
                    var state = tuple.start;
                    foreach (var op in tuple.ops)
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
                },
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Writer Monad Comparison");
}
