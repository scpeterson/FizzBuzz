using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public class LanguageExtCurryingComparisonDemo : IDemo
{
    public const string DemoKey = "langext-currying-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "currying", "triad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        (from amount in CurryingTriadRules.ParseBaseAmount(number)
            from rates in CurryingTriadRules.ResolveRates(name)
            select CurryingTriadRules.CurriedTotal(amount)(rates.DiscountRate)(rates.TaxRate))
        .Map(_ => unit);
}
