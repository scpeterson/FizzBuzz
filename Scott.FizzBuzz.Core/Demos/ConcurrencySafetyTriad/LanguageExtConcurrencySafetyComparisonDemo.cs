using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public class LanguageExtConcurrencySafetyComparisonDemo : IDemo
{
    public const string DemoKey = "langext-concurrency-safety-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "concurrency", "safety"];
    public string Description => "Pure fold-based update model that avoids shared mutable state and lost updates.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ConcurrencySafetyRules.ParseIterations(number)
            .Map(ConcurrencySafetyRules.ExecuteLanguageExtPure)
            .Bind(result =>
                result.FinalBalance == result.ExpectedBalance
                    ? Right<string, ConcurrencySafetyRules.ConcurrencySimulationResult>(result)
                    : Left<string, ConcurrencySafetyRules.ConcurrencySimulationResult>("Unexpected lost update in pure model."))
            .Map(_ => unit);
}
