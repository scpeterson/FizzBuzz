using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.BaselineLanguageExt;

public class LanguageExtDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "lang-ext";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "either", "baseline"];
    public string Description => "Baseline LanguageExt FizzBuzz demo using Either values to represent words versus numeric results.";
    public DemoExecutionResult Run(string? name, string? number)
    {
        return ExecuteWithSpacing(_output, () =>
        {
            // 1…100 → apply FizzBuzz → print each result
            Range(1, 100)
                .Map(LanguageExtFizzBuzz)
                .Iter(e =>
                    e.Match(
                        Left:  msg => _output.WriteLine(msg),
                        Right: num => _output.WriteLine(num.ToString())
                    ));
        }, "LanguageExt FizzBuzz");
    }
    
    /// <summary>
    /// Returns Left(text) for Fizz/Buzz/FizzBuzz,
    /// or Right(n) for plain numbers.
    /// </summary>
    private static Either<string, int> LanguageExtFizzBuzz(int value) =>
        (value % 3, value % 5) switch
        {
            (0, 0) => Left<string, int>("FizzBuzz"),
            (0, _) => Left<string, int>("Fizz"),
            (_, 0) => Left<string, int>("Buzz"),
            _      => Right<string, int>(value)
        };
}
