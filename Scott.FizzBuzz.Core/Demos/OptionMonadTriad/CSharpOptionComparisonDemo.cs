using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.OptionMonadTriad;

public class CSharpOptionComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpOptionComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpOptionComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-option-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "option", "monad"];
    public string Description => "Nullable-reference composition with custom Bind helpers to emulate Option flow.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var customer = OptionMonadSampleData.ResolveCustomer(name);

            var segment = customer
                .Bind(c => c.Profile)
                .Bind(profile => NormalizeEmail(profile.Email))
                .Bind(GetDomain)
                .Bind(OptionMonadSampleData.LookupSegment);

            var message = segment is null
                ? "No segment resolved (required custom nullable Bind + null-returning helpers)."
                : $"Segment resolved: {segment}.";

            _output.WriteLine(message);
            _output.WriteLine("C#/.NET comparison note: to mimic Option composition, we had to write custom Bind plumbing.");
        }, "C# Option Comparison");

    private static string? NormalizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return email.Trim();
    }

    private static string? GetDomain(string email)
    {
        var atIndex = email.IndexOf('@');
        if (atIndex <= 0 || atIndex == email.Length - 1)
        {
            return null;
        }

        return email[(atIndex + 1)..].ToLowerInvariant();
    }
}
