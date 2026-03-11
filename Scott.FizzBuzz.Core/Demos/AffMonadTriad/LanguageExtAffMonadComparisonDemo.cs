using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.AffMonadTriad;

public class LanguageExtAffMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-aff-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "aff", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        AffMonadRules.ComputeAff(name, number)
            .Run()
            .AsTask()
            .GetAwaiter()
            .GetResult()
            .Match(
                Succ: result => result.Map(_ => unit),
                Fail: error => Left<string, Unit>($"Aff failure: {error.Message}"));
}
