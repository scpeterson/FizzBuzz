using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public class LanguageExtEitherMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-either-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "either", "monad"];
    public string Description => "Either pipeline with short-circuiting error flow and no orchestration branching.";

    public Either<string, Unit> Run(string? name, string? number) =>
        EitherMonadRules.ParseAmount(number)
            .Bind(EitherMonadRules.ValidateAmountRange)
            .Bind(amount =>
                EitherMonadRules.ParseDiscountCode(name)
                    .Map(code => amount * EitherMonadRules.DiscountFactor(code)))
            .Bind(EitherMonadRules.EnsureMinimumCharge)
            .Map(_ => unit);
}
