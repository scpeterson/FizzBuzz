namespace Scott.FizzBuzz.Core.CommonExampleCode;

public static class FakeDatabase
{
    private static readonly Dictionary<int, Person> Db = new()
    {
        [1] = new Person { Id = 1, Name = "Alice", Age = 30 },
        [2] = new Person { Id = 2, Name = "Bob", Age = 25 },
    };

    public static Dictionary<int, Person> Persons => Db;
}