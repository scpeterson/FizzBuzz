using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public class LanguageExtWriterMonadComparisonDemo : IDemo
{
    public string Key => "langext-writer-monad-comparison";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "writer", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        (from start in WriterMonadRules.ParseStart(number)
            from ops in WriterMonadRules.ResolveOps(name)
            select WriterMonadRules.RunProgram(start, ops))
        .Map(program =>
        {
            _ = program.Run();
            return unit;
        });
}
