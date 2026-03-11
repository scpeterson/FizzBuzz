using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public class CSharpEventSourcingLiteComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpEventSourcingLiteComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpEventSourcingLiteComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-event-sourcing-lite-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "event-sourcing", "state", "database"];
    public string Description => "Pipeline-style replay and append with immutable projections using C# pattern matching and aggregation.";

    public Either<string, Unit> Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = EventSourcingLiteRules.ParseStreamId(name)
                .Bind(streamId =>
                    EventSourcingLiteRules.ParseDepositAmount(number)
                        .Map(depositAmount => EventSourcingLiteRules.ExecuteCSharpPipeline(streamId, depositAmount)));

            result.Match(
                Right: success => _output.WriteLine(EventSourcingLiteRules.FormatSummary(success)),
                Left: error => _output.WriteLine($"Failed: {error}"));
        }, "C# Event Sourcing Lite Comparison");
}
