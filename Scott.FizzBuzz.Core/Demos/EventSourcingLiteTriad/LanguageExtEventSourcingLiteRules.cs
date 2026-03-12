using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public static class LanguageExtEventSourcingLiteRules
{
    public static Either<string, string> ParseStreamId(string? value) =>
        EventSourcingLiteRules.TryParseStreamId(value, out var streamId, out var error)
            ? Right<string, string>(streamId!)
            : Left<string, string>(error ?? "Stream id is required.");

    public static Either<string, int> ParseDepositAmount(string? value) =>
        EventSourcingLiteRules.TryParseDepositAmount(value, out var amount, out var error)
            ? Right<string, int>(amount)
            : Left<string, int>(error ?? "Deposit amount must be an integer.");

    public static EventSourcingLiteRules.AccountProjection ProjectLanguageExt(Seq<EventSourcingLiteRules.AccountEvent> history) =>
        history.Fold(
            new EventSourcingLiteRules.AccountProjection(Opened: false, Balance: 0, Version: 0),
            (state, evt) => evt switch
            {
                EventSourcingLiteRules.AccountOpened => state with { Opened = true, Version = state.Version + 1 },
                EventSourcingLiteRules.FundsDeposited deposit => state with { Balance = state.Balance + deposit.Amount, Version = state.Version + 1 },
                EventSourcingLiteRules.FundsWithdrawn withdrawal => state with { Balance = state.Balance - withdrawal.Amount, Version = state.Version + 1 },
                _ => state
            });

    public static EventSourcingLiteRules.EventSourcingResult ExecuteLanguageExtPipeline(string streamId, int depositAmount)
    {
        var history = toSeq(EventSourcingLiteRules.SeedHistory(streamId));
        var before = ProjectLanguageExt(history);

        var withOpen = before.Opened
            ? history
            : history.Add(new EventSourcingLiteRules.AccountOpened(streamId));

        var afterHistory = withOpen.Add(new EventSourcingLiteRules.FundsDeposited(depositAmount));
        var after = ProjectLanguageExt(afterHistory);

        return new EventSourcingLiteRules.EventSourcingResult(streamId, before, after, before.Version, after.Version);
    }
}
