using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.IOMonadTriad;

public class LanguageExtIoMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-io-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "io", "monad"];
    public string Description => "LanguageExt IO-style composition using Eff (v4 equivalent) for effectful workflow sequencing.";

    public Either<string, Unit> Run(string? name, string? number)
    {
        var program =
            from profile in Eff(() => IoMonadRules.ResolveProfile(name))
            from weight in Eff(() => IoMonadRules.ParseWeight(number))
            select
                from p in profile
                from w in weight
                select IoMonadRules.CalculateQuote(w, p);

        return program
            .Run()
            .Match(
                Succ: result => result.Map(_ => unit),
                Fail: error => Left<string, Unit>($"IO effect failure: {error.Message}"));
    }
}
