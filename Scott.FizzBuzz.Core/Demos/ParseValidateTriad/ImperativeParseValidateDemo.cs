using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.ParseValidateTriad;

public class ImperativeParseValidateDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeParseValidateDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeParseValidateDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-parse-validate";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "validation", "parsing"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var input = number ?? "12";

            if (!int.TryParse(input, out var parsed))
            {
                _output.WriteLine("Not an integer.");
                return;
            }

            if (parsed <= 0)
            {
                _output.WriteLine("Must be > 0.");
                return;
            }

            _output.WriteLine($"Accepted: {parsed}");
        }, "Imperative Parse + Validate");
}
