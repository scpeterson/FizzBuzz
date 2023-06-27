// See https://aka.ms/new-console-template for more information

using CommandLine;
using LanguageExt;
using Scott.FizzBuzz.Console;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.AffExamples;
using Scott.FizzBuzz.Core.CommonExampleCode;
using Scott.FizzBuzz.Core.EffExamples;
using Scott.FizzBuzz.Core.EitherExamples;
using Scott.FizzBuzz.Core.OptionExample;
using Scott.FizzBuzz.Core.TryOptionExamples;
using static Scott.FizzBuzz.Core.MathExamples;
using static Scott.FizzBuzz.Core.ImperativeExample;
using static Scott.FizzBuzz.Core.NoDependencyExample;
using static Scott.FizzBuzz.Core.LanguageExtExample;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.DatabaseExample.DatabaseShell;

var methodMap = new Dictionary<string, Action>
{
    {"imperative", ShowImperativeFizzBuzz},
    {"no-dependency", ShowNoDependencyExample},
    {"basic-either", ShowBasicEither}
};

var parser = new Parser(with => with.HelpWriter = Console.Error);

parser.ParseArguments<Options>(args)
    .WithParsed<Options>(opts =>
    {
        if (methodMap.TryGetValue(opts.Method, out var method))
        {
            method();
        }
        else
        {
            Console.WriteLine("Invalid method name entered.");
        }
    })
    .WithNotParsed(errors =>
    {
        Console.WriteLine("Failed to parse command line arguments due to the following errors:");
        foreach (var error in errors)
        {
            Console.WriteLine($"- {error.Tag}: {error.ToString()}");
        }
    });



//ShowImperativeFizzBuzz();
//ShowNoDependencyExample();
//ShowLanguageExtFizzBuzz();
//ShowMath();
//ShowMathWithPipe();
//ShowBasicEither();
//ShowOption();
//ShowTryOption();
//ShowAff();
//ShowEff();
//ShowDatabase();

static void ShowImperativeFizzBuzz()
{
    //Old school, imperative
    for (var i = 1; i <= 100; i++)
    {
        var result = ImperativeFizzBuzz(i);
        Console.WriteLine(result);
    }
}

void ShowNoDependencyExample()
{
    //Modern C#, using LINQ and pattern matching with NO other dependencies
    Enumerable.Range(1, 100)
        .Select(x => NoDependencyFizzBuzz(x))
        .ToList()
        .ForEach(x => Console.WriteLine(x));
}

void ShowLanguageExtFizzBuzz()
{
    //Using LanguageExt dependency and Monads
    Range(1, 100)
        .Map(x => LanguageExtFizzBuzz(x))
        .Iter(result =>  
            result.Match(
                Left: x => Console.WriteLine(x),
                Right: x => Console.WriteLine(x.ToString())));    
}

void ShowMath()
{
    Console.WriteLine(AddOne(5));
    Console.WriteLine(TimesTwo(AddOne(5)));
    Console.WriteLine(Square(TimesTwo(AddOne(5))));
}

void ShowMathWithPipe()
{
    Console.WriteLine(5.Pipe(AddOne));
    Console.WriteLine(5.Pipe(AddOne).Pipe(TimesTwo));
    Console.WriteLine(5.Pipe(AddOne).Pipe(TimesTwo).Pipe(Square));
}

void ShowBasicEither()
{
    // Use Match to handle the possible outcomes
    var result = UserRepository.GetUser("Fred");
    var output = result.Match(
        Right: user => $"Found user: {user.Name}, {user.Age}",
        Left: error => error.Message
    );

    Console.WriteLine(output);
}

void ShowOption()
{
    var repo = new UserRepositoryOption();

    // Use Match to handle the possible outcomes
    var result = repo.GetUser("Alice");
    var output = result.Match(
            Some: user => $"Found user: {user.Name}, {user.Age}",
            None: () => "No user found"
        );

    Console.WriteLine(output);
}

void ShowTryOption()
{
    var repo = new UserRepositoryForTryOption();

    // Use Match to handle the possible outcomes
    var result = repo.GetUser("Fred");
    var output = result
        .Match(
            Some: user => $"Found user: {user.Name}, {user.Age}",
            None: () => "No user found",
            Fail: ex => $"An error occurred: {ex.Message}"
        );

    Console.WriteLine(output);
}

void ShowAff()
{
    var repo = new UserRepositoryAff();

    // Run the Aff and handle the possible outcomes
    var result = repo.GetUserAsync("Fred").Run().Result
        .Match(
            Succ: user => $"Found user: {user.Name}, {user.Age}",
            Fail: ex => $"An error occurred: {ex.Message}"
        );

    Console.WriteLine(result);
}

void ShowEff()
{
    var repo = new UserRepositoryEff();

    // Use Match to handle the possible outcomes
    var result = repo.GetUser("Alice")
        .Match(
            Some: user => $"Found user: {user.Name}, {user.Age}",
            None: () => "No user found"
        );

    // Run the Eff
    var eff = WriteToConsole(result).Run();
}

void ShowDatabase()
{
    const int personId = 1; // assume we want to get person with Id 1

    // Run the GetPerson Eff to retrieve the person
    var result = GetPerson(personId).Run();
    
    //Match on the result
    result.Match(
        Succ: person => WriteToConsole(person.Name).Run(),
        Fail: error => WriteToConsole($"Failed to get person: {error.Message}").Run());
}


// Eff for writing a line to the console
static Eff<Unit> WriteToConsole(string message) =>
    Eff<Unit>(() =>
    {
        Console.WriteLine(message);
        return unit;
    });
