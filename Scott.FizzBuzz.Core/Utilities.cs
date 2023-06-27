namespace Scott.FizzBuzz.Core;

public static class Utilities
{
    public static U Pipe<T, U>(this T input, Func<T, U> func)
    {
        var result = func(input);
        return result;
    }
}