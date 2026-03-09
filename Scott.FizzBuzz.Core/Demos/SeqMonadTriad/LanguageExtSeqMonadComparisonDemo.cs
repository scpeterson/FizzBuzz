using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public class LanguageExtSeqMonadComparisonDemo : IDemo
{
    public string Key => "langext-seq-monad-comparison";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "seq", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        (from numbers in SeqMonadRules.ResolveNumbers(name)
            from threshold in SeqMonadRules.ParseThreshold(number)
            select SeqMonadRules.Compute(numbers, threshold))
        .Map(_ => unit);
}
