using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.BaselineLanguageExt;

public static class EitherValidation
{
    public static Either<string, int> ImperativeValidate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Left("Number is required.");
        if (!int.TryParse(input, out var number))
            return Left("Not a valid integer.");
        if (number >= 10)
            return Left("Must be less than 10.");
        return Right(number);
    }

    public static Either<string, int> FunctionalValidate(string? input) =>
        ValidateNotEmpty(input)
            .Bind(ParseInt)
            .Bind(CheckLessThan(10));

    public static Either<string, string> ValidateNotEmpty(string? input) =>
        !string.IsNullOrWhiteSpace(input)
            ? Right(input!)
            : Left<string, string>("Number is required.");

    public static Either<string, int> ParseInt(string input) =>
        parseInt(input).ToEither("Not a valid integer.");

    public static Func<int, Either<string, int>> CheckLessThan(int limit) =>
        number => number < limit
            ? Right<string, int>(number)
            : Left<string, int>($"Must be less than {limit}.");
}
