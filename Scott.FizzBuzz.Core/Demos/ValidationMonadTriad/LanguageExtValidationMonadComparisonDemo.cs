using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ValidationMonadTriad;

public class LanguageExtValidationMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-validation-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "validation", "monad"];
    public string Description => "LanguageExt Validation applicative composition with error accumulation.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ValidationMonadRules.ValidateCandidate(name, number)
            .ToEither()
            .MapLeft(error => error.ToString())
            .Map(_ => unit);
}
