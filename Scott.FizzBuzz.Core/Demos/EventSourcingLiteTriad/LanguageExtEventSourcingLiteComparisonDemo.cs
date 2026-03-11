using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public class LanguageExtEventSourcingLiteComparisonDemo : IDemo
{
    public const string DemoKey = "langext-event-sourcing-lite-comparison";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "event-sourcing", "state", "database"];
    public string Description => "Pure event stream replay and command handling via immutable Seq and fold-based projection.";

    public Either<string, Unit> Run(string? name, string? number) =>
        EventSourcingLiteRules.ParseStreamId(name)
            .Bind(streamId =>
                EventSourcingLiteRules.ParseDepositAmount(number)
                    .Map(depositAmount => EventSourcingLiteRules.ExecuteLanguageExtPipeline(streamId, depositAmount)))
            .Map(_ => unit);
}
