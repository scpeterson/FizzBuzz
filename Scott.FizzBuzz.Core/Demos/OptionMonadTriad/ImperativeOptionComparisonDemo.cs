using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.OptionMonadTriad;

public class ImperativeOptionComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeOptionComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeOptionComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-option-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "option", "monad"];
    public string Description => "Manual nested null checks to resolve a customer segment from optional data.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var checks = 0;
            var customer = OptionMonadSampleData.ResolveCustomer(name);

            checks++;
            if (customer is null)
            {
                _output.WriteLine("Customer scenario not found.");
                _output.WriteLine($"Imperative defensive branches: {checks}");
                return;
            }

            checks++;
            if (customer.Profile is null)
            {
                _output.WriteLine($"{customer.Name}: no profile.");
                _output.WriteLine($"Imperative defensive branches: {checks}");
                return;
            }

            var email = customer.Profile.Email;
            checks++;
            if (string.IsNullOrWhiteSpace(email))
            {
                _output.WriteLine($"{customer.Name}: no usable email.");
                _output.WriteLine($"Imperative defensive branches: {checks}");
                return;
            }

            var trimmedEmail = email.Trim();
            var atIndex = trimmedEmail.IndexOf('@');
            checks++;
            if (atIndex <= 0 || atIndex == trimmedEmail.Length - 1)
            {
                _output.WriteLine($"{customer.Name}: malformed email '{trimmedEmail}'.");
                _output.WriteLine($"Imperative defensive branches: {checks}");
                return;
            }

            var domain = trimmedEmail[(atIndex + 1)..];
            var segment = OptionMonadSampleData.LookupSegment(domain);
            checks++;
            if (segment is null)
            {
                _output.WriteLine($"{customer.Name}: no segment mapping for domain '{domain}'.");
                _output.WriteLine($"Imperative defensive branches: {checks}");
                return;
            }

            _output.WriteLine($"{customer.Name}: segment = {segment}.");
            _output.WriteLine($"Imperative defensive branches: {checks}");
        }, "Imperative Option Comparison");
}
