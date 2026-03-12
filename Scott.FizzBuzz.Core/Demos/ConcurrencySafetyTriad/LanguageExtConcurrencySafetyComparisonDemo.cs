using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.ConcurrencySafetyTriad;

public class LanguageExtConcurrencySafetyComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtConcurrencySafetyComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtConcurrencySafetyComparisonDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-concurrency-safety-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "concurrency", "safety"];
    public string Description => "Pure fold-based update model that avoids shared mutable state and lost updates.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Concurrency Safety Comparison",
            ComputeResult(number),
            (output, result) =>
            {
                output.WriteLine("Result: no lost updates.");
                output.WriteLine(ConcurrencySafetyRules.FormatSummary(result));
            });

    private static Either<string, ConcurrencySafetyRules.ConcurrencySimulationResult> ComputeResult(string? number) =>
        LanguageExtConcurrencySafetyRules.ParseIterations(number)
            .Map(LanguageExtConcurrencySafetyRules.ExecuteLanguageExtPure)
            .Bind(result =>
                result.FinalBalance == result.ExpectedBalance
                    ? Right<string, ConcurrencySafetyRules.ConcurrencySimulationResult>(result)
                    : Left<string, ConcurrencySafetyRules.ConcurrencySimulationResult>("Unexpected lost update in pure model."));
}
