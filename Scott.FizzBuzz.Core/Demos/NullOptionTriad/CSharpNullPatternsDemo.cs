using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.NullOptionTriad;

public class CSharpNullPatternsDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpNullPatternsDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpNullPatternsDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-null-patterns";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "null"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var message = name switch
            {
                null => "Name missing.",
                { Length: 0 } => "Name empty.",
                _ => $"Hello, {name.Trim()}."
            };

            _output.WriteLine(message);
        }, "C# Null Patterns");
}
