using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core;

public static class LanguageExtExample
{
    public static Either<string, int> LanguageExtFizzBuzz(int value)
    {
        return (value % 3, value % 5) switch
        {
            (0, 0) => Left("LanguageExtFizzBuzz"),
            (0, _) => Left("LanguageExtFizz"),
            (_, 0) => Left("LanguageExtBuzz"),
            _ => Right(value)
        };
    }
}