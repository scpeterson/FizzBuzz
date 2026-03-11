using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;

public class LanguageExtReaderMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-reader-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "reader", "monad"];
    public string Description => "Reader monad composes environment-dependent functions without passing context explicitly.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ReaderMonadRules.ResolveContext(name)
            .Bind(context => ReaderMonadRules.ParseSubtotal(number).Map(subtotal => (context, subtotal)))
            .Bind(args =>
            {
                var program = BuildProgram(args.subtotal);
                var fin = program.Run(args.context);

                return fin.Match(
                    Succ: _ => Right<string, Unit>(unit),
                    Fail: error => Left<string, Unit>($"Reader failure: {error.Message}"));
            });

    private static Reader<ReaderPricingContext, string> BuildProgram(decimal subtotal) =>
        from taxRate in Reader<ReaderPricingContext, decimal>(ctx => ctx.TaxRate)
        from fee in Reader<ReaderPricingContext, decimal>(ctx => ctx.ServiceFee)
        from profile in Reader<ReaderPricingContext, string>(ctx => ctx.ProfileName)
        from currency in Reader<ReaderPricingContext, string>(ctx => ctx.Currency)
        let total = (subtotal * (1m + taxRate)) + fee
        select $"{profile}: total = {total:0.00} {currency}";
}
