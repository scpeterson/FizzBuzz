using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.SeqMonadTriad;

public class ImperativeSeqMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeSeqMonadComparisonDemo() : this(new ConsoleOutput()) { }

    public ImperativeSeqMonadComparisonDemo(IOutput output) => _output = output;

    public string Key => "imperative-seq-monad-comparison";
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "seq", "monad"];

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!int.TryParse(number, out var threshold))
            {
                _output.WriteLine("Failed: Threshold must be numeric.");
                return;
            }

            var numbersEither = SeqMonadRules.ResolveNumbers(name);
            if (numbersEither.IsLeft)
            {
                _output.WriteLine($"Failed: {numbersEither.LeftToList()[0]}");
                return;
            }

            var sum = 0;
            foreach (var n in numbersEither.RightToList()[0])
            {
                if (n >= threshold)
                {
                    sum += n * n;
                }
            }

            _output.WriteLine($"Sum: {sum}");
        }, "Imperative Seq Monad Comparison");
}
