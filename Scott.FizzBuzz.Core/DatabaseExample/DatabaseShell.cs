using LanguageExt;
using Scott.FizzBuzz.Core.CommonExampleCode;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.DatabaseExample;

public static class DatabaseShell
{
    public static Eff<Person> GetPerson(int id) =>
        Eff<Person>(() =>
        {
            if (FakeDatabase.Persons.TryGetValue(id, out var person))
            {
                return person;
            }

            throw new KeyNotFoundException($"No person found with id {id}");
        });
    
    public static Eff<Unit> AddPerson(Person person) =>
        Eff<Unit>(() =>
        {
            var newId = FakeDatabase.Persons.Keys.DefaultIfEmpty(0).Max() + 1;
            person.Id = newId;
            FakeDatabase.Persons[newId] = person;
            return unit;
        });

    public static Eff<Unit> UpdatePerson(int id, Person person) =>
        Eff<Unit>(() =>
        {
            if (FakeDatabase.Persons.ContainsKey(id))
            {
                FakeDatabase.Persons[id] = person;
                return unit;
            }

            throw new KeyNotFoundException($"No person found with id {id}");
        });

}