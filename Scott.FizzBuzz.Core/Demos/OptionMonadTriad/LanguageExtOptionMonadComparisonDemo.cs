using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.OptionMonadTriad;

public class LanguageExtOptionMonadComparisonDemo : IDemo
{
    public const string DemoKey = "langext-option-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "option", "monad"];
    public string Description => "Option pipeline for nested optional data with no null-check branching in orchestration.";

    public Either<string, Unit> Run(string? name, string? number) =>
        Optional(OptionMonadSampleData.ResolveCustomer(name))
            .Bind(customer => Optional(customer.Profile))
            .Bind(profile => Optional(profile.Email))
            .Map(email => email.Trim())
            .Filter(email => email.Length > 0)
            .Bind(ParseDomain)
            .Bind(domain => Optional(OptionMonadSampleData.LookupSegment(domain)))
            .ToEither("No segment resolved from optional inputs.")
            .Map(_ => unit);

    private static Option<string> ParseDomain(string email)
    {
        var atIndex = email.IndexOf('@');
        if (atIndex <= 0 || atIndex == email.Length - 1)
        {
            return None;
        }

        return Some(email[(atIndex + 1)..].ToLowerInvariant());
    }
}
