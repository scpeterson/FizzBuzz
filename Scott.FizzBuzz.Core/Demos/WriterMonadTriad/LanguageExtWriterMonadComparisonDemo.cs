using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public class LanguageExtWriterMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtWriterMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtWriterMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-writer-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "writer", "monad"];
    public string Description => "LanguageExt Writer composition that carries log output alongside computed state.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Writer Monad Comparison",
            ComputeResult(name, number),
            (output, result) =>
            {
                output.WriteLine($"Final state: {result.FinalState}");
                foreach (var entry in result.Logs)
                {
                    output.WriteLine(entry);
                }
            });

    private static Either<string, (int FinalState, Seq<string> Logs)> ComputeResult(string? name, string? number) =>
        from start in LanguageExtWriterMonadRules.ParseStart(number)
        from ops in LanguageExtWriterMonadRules.ResolveOps(name)
        let run = LanguageExtWriterMonadRules.RunProgram(start, ops).Run()
        select (ifNoneOrFail(run.Value, () => start, _ => start), run.Output);
}
