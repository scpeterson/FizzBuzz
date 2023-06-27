using LanguageExt;
using Scott.FizzBuzz.Core.CommonExampleCode;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.AffExamples;

public class UserRepositoryAff
{
    // Use Aff to model an asynchronous function that could either return a User or throw an Exception
    public Aff<User> GetUserAsync(string name)
    {
        return Aff<User>(
            async () =>
            {
                // Simulate async work, such as fetching data from a database
                await Task.Delay(1000);

                // For demonstration purposes, let's assume we have a database with users
                Dictionary<string, User> database = new()
                {
                    ["Alice"] = new User { Name = "Alice", Age = 25 },
                    ["Bob"] = new User { Name = "Bob", Age = 30 }
                };

                if (database.TryGetValue(name, out var user))
                {
                    return user;
                }

                throw new KeyNotFoundException($"No user found with name {name}");
            }
        );
    }
}