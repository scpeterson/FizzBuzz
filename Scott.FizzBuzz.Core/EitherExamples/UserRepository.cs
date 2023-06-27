using LanguageExt;
using Scott.FizzBuzz.Core.CommonExampleCode;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.EitherExamples;

public static class UserRepository
{
    // Use Either to model a function that could either return a User or an Error
    public static Either<Error, User> GetUser(string name)
    {
        // For demonstration purposes, let's assume we have a database with users
        Dictionary<string, User> database = new()
        {
            ["Alice"] = new User { Name = "Alice", Age = 25 },
            ["Bob"] = new User { Name = "Bob", Age = 30 }
        };

        // Use TryGetValue to more efficiently check for the value
        return database.TryGetValue(name, out var user) 
            ? Right<Error, User>(user) 
            : Left<Error, User>(new Error { Message = $"No user found with name {name}" });
    }
}