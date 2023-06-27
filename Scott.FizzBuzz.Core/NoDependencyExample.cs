namespace Scott.FizzBuzz.Core;

public static class NoDependencyExample
{
    public static string NoDependencyFizzBuzz(int value)
    {
        return (value % 3, value % 5) switch
        {
            (0, 0) => "NoDependencyFizzBuzz",
            (0, _) => "NoDependencyFizz",
            (_, 0) => "NoDependencyBuzz",
            _ => value.ToString()
        };
    }
}