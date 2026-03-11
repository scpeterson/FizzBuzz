using LanguageExt;
using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public class LanguageExtCompositionRootComparisonDemo : IDemo
{
    public const string DemoKey = "langext-composition-root";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "composition-root", "triad"];

    public Either<string, Unit> Run(string? name, string? number)
    {
        var env = new InMemoryFunctionalDemoEnvironment();
        var tier = CompositionRootRules.NormalizeTier(name);
        var normalizedTier = tier is "standard" or "vip" or "employee" ? tier : "standard";

        return CompositionRootRules.ParseAmount(number)
            .Bind(amount =>
            {
                var readerResult = CompositionRootRules.QuoteReader(amount, normalizedTier, "us").Run(env);
                return readerResult.Match(
                    Succ: total => total.Map(_ => unit),
                    Fail: error => Left<string, Unit>($"Reader failure: {error.Message}"));
            });
    }
}
