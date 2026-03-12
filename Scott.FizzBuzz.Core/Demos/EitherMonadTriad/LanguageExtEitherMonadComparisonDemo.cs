using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Demos.EitherMonadTriad;

public class LanguageExtEitherMonadComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtEitherMonadComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtEitherMonadComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-either-monad-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "either", "monad"];
    public string Description => "Either pipeline with short-circuiting error flow and no orchestration branching.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Either Monad Comparison",
            ComputeResult(name, number),
            (output, result) => output.WriteLine($"Result: {result:0.00}"));

    private static Either<string, decimal> ComputeResult(string? name, string? number) =>
        LanguageExtEitherMonadRules.ParseAmount(number)
            .Bind(LanguageExtEitherMonadRules.ValidateAmountRange)
            .Bind(amount =>
                LanguageExtEitherMonadRules.ParseDiscountCode(name)
                    .Map(code => amount * EitherMonadRules.DiscountFactor(code)))
            .Bind(LanguageExtEitherMonadRules.EnsureMinimumCharge);
}
