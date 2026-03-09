using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryMonadTriad;

public class LanguageExtTryMonadComparisonDemo : IDemo
{
    public string Key => "langext-try-monad-comparison";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "try", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        TryMonadRules.ParseInput(number)
            .Bind(value =>
                TryMonadRules.InverseTry(value)
                    .Match(
                        Succ: inverse => Right<string, decimal>(inverse),
                        Fail: ex => Left<string, decimal>(ex.Message)))
            .Map(_ => unit);
}
