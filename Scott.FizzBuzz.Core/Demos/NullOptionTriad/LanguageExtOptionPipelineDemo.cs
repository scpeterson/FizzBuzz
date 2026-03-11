using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.NullOptionTriad;

public class LanguageExtOptionPipelineDemo : IDemo
{
    public const string DemoKey = "langext-option-pipeline";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "option", "null"];

    public Either<string, Unit> Run(string? name, string? number) =>
        Optional(name)
            .Map(value => value.Trim())
            .Filter(value => value.Length > 0)
            .ToEither("Name missing or empty.")
            .Map(_ => unit);
}
