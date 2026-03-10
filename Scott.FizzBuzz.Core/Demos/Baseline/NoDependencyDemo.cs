using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.NoDependencyExample;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.Baseline;

public class NoDependencyDemo : IDemo
{
    private readonly IOutput _output;

    public NoDependencyDemo() : this(new ConsoleOutput())
    {
    }

    public NoDependencyDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => "no-dependency";
    public string Category => "general";
    public IReadOnlyCollection<string> Tags => ["general", "baseline"];
    
    /// <summary>
    /// Returns either an error (Left) or a single string containing all the lines to print (Right).
    /// </summary>
    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, ShowNoDependencyExample, nameof(ShowNoDependencyExample));

    private void ShowNoDependencyExample()
    {
        // Use LanguageExt Seq pipeline to avoid intermediate list materialization.
        Range(1, 100)
            .Map(NoDependencyFizzBuzz)
            .Iter(_output.WriteLine);
    }
}
