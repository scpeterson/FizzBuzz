using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ReaderMonadTriad;

public static class LanguageExtReaderMonadRules
{
    public static Either<string, decimal> ParseSubtotal(string? input) =>
        ReaderMonadRules.TryParseSubtotal(input, out var subtotal, out var error)
            ? Right<string, decimal>(subtotal)
            : Left<string, decimal>(error ?? "Subtotal must be a valid decimal value.");

    public static Either<string, ReaderPricingContext> ResolveContext(string? profile) =>
        ReaderMonadRules.TryResolveContext(profile, out var context, out var error)
            ? Right<string, ReaderPricingContext>(context!)
            : Left<string, ReaderPricingContext>(error ?? "Unknown pricing profile. Use standard, vip, or intl.");
}
