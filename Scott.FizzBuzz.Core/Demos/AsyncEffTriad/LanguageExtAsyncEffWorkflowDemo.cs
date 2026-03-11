using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.AsyncEffTriad;

public class LanguageExtAsyncEffWorkflowDemo : IDemo
{
    public const string DemoKey = "langext-eff-async-workflow";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "async", "eff", "aff", "effects"];
    public string Description => "LanguageExt composition using Eff (sync effect) + Aff (async effect).";

    public Either<string, Unit> Run(string? name, string? number)
    {
        var effResult = ParseAndDoubleEff(number)
            .Run()
            .Match(
                Succ: result => result,
                Fail: error => Left<string, int>($"Eff failure: {error.Message}"));

        var affResult = AddTenAff(effResult)
            .Run()
            .AsTask()
            .GetAwaiter()
            .GetResult()
            .Match(
                Succ: result => result,
                Fail: error => Left<string, int>($"Aff failure: {error.Message}"));

        return affResult.Map(_ => unit);
    }

    private static Eff<Either<string, int>> ParseAndDoubleEff(string? input) =>
        Eff(() => ValidateInput(input).Map(value => value * 2));

    private static Aff<Either<string, int>> AddTenAff(Either<string, int> current) =>
        Aff<Either<string, int>>(async () =>
        {
            await Task.Delay(10);
            return current.Map(value => value + 10);
        });

    private static Either<string, int> ValidateInput(string? input) =>
        !string.IsNullOrWhiteSpace(input) && int.TryParse(input, out var parsed)
            ? Right<string, int>(parsed)
            : Left<string, int>("Input must be a non-empty integer.");
}
