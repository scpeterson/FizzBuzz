using CommandLine;

namespace Scott.FizzBuzz.Core;

public static class ImperativeExample
{
    public static string ImperativeFizzBuzz(int value)
    {
        if (value % 3 == 0 && value % 5 == 0)
        {
            return "ImperativeFizzBuzz";
        }

        if (value % 3 == 0)
        {
            return "ImperativeFizz";
        }

        if (value % 5 == 0)
        {
            return "ImperativeBuzz";
        }

        return value.ToString();
    }
}