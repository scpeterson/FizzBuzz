using LanguageExt;
using Scott.FizzBuzz.Core.CommonExampleCode;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.TryOptionExamples;

public class UserRepositoryForTryOption
{
    // Use TryOption to model a function that could either return a User, None or throw an Exception
    public TryOption<User> GetUser(string name)
    {
        return TryOption<User>(() =>
        {
            // For demonstration purposes, let's assume we have a database with users
            Dictionary<string, User> database = new()
            {
                ["Alice"] = new User { Name = "Alice", Age = 25 },
                ["Bob"] = new User { Name = "Bob", Age = 30 }
            };

            // Use TryGetValue to more efficiently check for the value
            if (database.TryGetValue(name, out var user))
            {
                return Some(user);
            }
            else
            {
                return None;
            }
        });
    }
}