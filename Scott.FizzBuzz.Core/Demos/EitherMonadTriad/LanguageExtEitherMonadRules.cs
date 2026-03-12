using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public static class LanguageExtEitherMonadRules
{
    public static Either<string, decimal> ParseAmount(string? input) =>
        EitherMonadRules.TryParseAmount(input, out var amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Amount must be a valid decimal value.");

    public static Either<string, decimal> ValidateAmountRange(decimal amount) =>
        EitherMonadRules.TryValidateAmountRange(amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Amount must be between 1 and 1000.");

    public static Either<string, EitherDiscountCode> ParseDiscountCode(string? code) =>
        EitherMonadRules.TryParseDiscountCode(code, out var parsedCode, out var error)
            ? Right<string, EitherDiscountCode>(parsedCode)
            : Left<string, EitherDiscountCode>(error ?? "Unknown discount code. Use vip, student, or employee.");

    public static Either<string, decimal> EnsureMinimumCharge(decimal amount) =>
        EitherMonadRules.TryEnsureMinimumCharge(amount, out var error)
            ? Right<string, decimal>(amount)
            : Left<string, decimal>(error ?? "Final amount is below the minimum charge.");
}
