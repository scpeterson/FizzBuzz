using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EffMonadTriad;

public class LanguageExtEffMonadComparisonDemo : IDemo
{
    public string Key => "langext-eff-monad-comparison";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "eff", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        EffMonadRules.ComputeEff(name, number)
            .Run()
            .Match(
                Succ: result => result.Map(_ => unit),
                Fail: error => Left<string, Unit>($"Eff failure: {error.Message}"));
}
