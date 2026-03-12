using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.OptionMonadTriad;

public class LanguageExtOptionMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtOptionMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtOptionMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-option-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "option", "monad"];
    public string Description => "Option pipeline for nested optional data with no null-check branching in orchestration.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Option Monad Comparison",
            ComputeResult(name),
            (output, result) => output.WriteLine($"Result: {result}"));

    private static Either<string, string> ComputeResult(string? name) =>
        Optional(OptionMonadSampleData.ResolveCustomer(name))
            .Bind(customer => Optional(customer.Profile))
            .Bind(profile => Optional(profile.Email))
            .Map(email => email.Trim())
            .Filter(email => email.Length > 0)
            .Bind(ParseDomain)
            .Bind(domain =>
                Optional(OptionMonadSampleData.LookupSegment(domain))
                    .Map(segment => $"segment = {segment}"))
            .ToEither("No segment resolved from optional inputs.")
        ;

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
