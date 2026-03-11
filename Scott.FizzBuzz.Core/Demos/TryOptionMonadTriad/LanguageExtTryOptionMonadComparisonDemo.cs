using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.TryOptionMonadTriad;

public class LanguageExtTryOptionMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-tryoption-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "tryoption", "monad"];

    public Either<string, Unit> Run(string? name, string? number)
    {
        var result =
            from id in TryOptionMonadRules.ParseId(number)
            select TryOptionMonadRules.LookupTryOption(id)
                .Map(value => $"Some:{value:0.##}");

        return result.Bind(computation =>
        {
            var output = ifNoneOrFail(
                computation,
                None: () => "None",
                Fail: ex => $"Fail:{ex.Message}");

            if (output.StartsWith("Some:", StringComparison.Ordinal))
            {
                return Right<string, Unit>(unit);
            }

            var message = output.StartsWith("Fail:", StringComparison.Ordinal)
                ? output[5..]
                : "No value for id.";

            return Left<string, Unit>(message);
        });
    }
}
