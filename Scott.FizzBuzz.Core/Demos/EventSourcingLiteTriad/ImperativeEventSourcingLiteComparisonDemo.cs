using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public class ImperativeEventSourcingLiteComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeEventSourcingLiteComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeEventSourcingLiteComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-event-sourcing-lite-comparison";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "event-sourcing", "state", "database"];
    public string Description => "Classic mutable event-list workflow: replay, append, replay again to derive current state.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            if (!EventSourcingLiteRules.TryParseStreamId(name, out var streamId, out var error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            if (!EventSourcingLiteRules.TryParseDepositAmount(number, out var depositAmount, out error))
            {
                _output.WriteLine($"Failed: {error}");
                return;
            }

            var result = EventSourcingLiteRules.ExecuteImperative(streamId!, depositAmount);
            _output.WriteLine("Result: event stream updated.");
            _output.WriteLine(EventSourcingLiteRules.FormatSummary(result));
        }, "Imperative Event Sourcing Lite Comparison");
}
