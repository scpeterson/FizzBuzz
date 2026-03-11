using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.EventSourcingLiteTriad;

public static class EventSourcingLiteRules
{
    public abstract record AccountEvent;
    public sealed record AccountOpened(string StreamId) : AccountEvent;
    public sealed record FundsDeposited(int Amount) : AccountEvent;
    public sealed record FundsWithdrawn(int Amount) : AccountEvent;

    public sealed record AccountProjection(bool Opened, int Balance, int Version);

    public sealed record EventSourcingResult(
        string StreamId,
        AccountProjection Before,
        AccountProjection After,
        int EventCountBefore,
        int EventCountAfter)
    {
        public int Delta => After.Balance - Before.Balance;
    }

    public static Either<string, string> ParseStreamId(string? value)
    {
        var streamId = (value ?? string.Empty).Trim();
        return string.IsNullOrWhiteSpace(streamId)
            ? Left<string, string>("Stream id is required.")
            : streamId.Length > 64
                ? Left<string, string>("Stream id must be 64 characters or fewer.")
                : Right<string, string>(streamId);
    }

    public static Either<string, int> ParseDepositAmount(string? value) =>
        int.TryParse(value, out var parsed)
            ? parsed is >= 1 and <= 10_000
                ? Right<string, int>(parsed)
                : Left<string, int>("Deposit amount must be between 1 and 10000.")
            : Left<string, int>("Deposit amount must be an integer.");

    public static Seq<AccountEvent> SeedHistory(string streamId) =>
        string.Equals(streamId, "new-stream", StringComparison.OrdinalIgnoreCase)
            ? Seq<AccountEvent>()
            : Seq<AccountEvent>(
                new AccountOpened(streamId),
                new FundsDeposited(30),
                new FundsWithdrawn(12),
                new FundsDeposited(5));

    public static AccountProjection ProjectImperative(IEnumerable<AccountEvent> history)
    {
        var opened = false;
        var balance = 0;
        var version = 0;

        foreach (var evt in history)
        {
            version++;

            switch (evt)
            {
                case AccountOpened:
                    opened = true;
                    break;
                case FundsDeposited deposit:
                    balance += deposit.Amount;
                    break;
                case FundsWithdrawn withdrawal:
                    balance -= withdrawal.Amount;
                    break;
            }
        }

        return new AccountProjection(opened, balance, version);
    }

    public static AccountProjection ProjectCSharpPipeline(IEnumerable<AccountEvent> history) =>
        history.Aggregate(
            new AccountProjection(Opened: false, Balance: 0, Version: 0),
            (state, evt) => evt switch
            {
                AccountOpened => state with { Opened = true, Version = state.Version + 1 },
                FundsDeposited deposit => state with { Balance = state.Balance + deposit.Amount, Version = state.Version + 1 },
                FundsWithdrawn withdrawal => state with { Balance = state.Balance - withdrawal.Amount, Version = state.Version + 1 },
                _ => state
            });

    public static AccountProjection ProjectLanguageExt(Seq<AccountEvent> history) =>
        history.Fold(
            new AccountProjection(Opened: false, Balance: 0, Version: 0),
            (state, evt) => evt switch
            {
                AccountOpened => state with { Opened = true, Version = state.Version + 1 },
                FundsDeposited deposit => state with { Balance = state.Balance + deposit.Amount, Version = state.Version + 1 },
                FundsWithdrawn withdrawal => state with { Balance = state.Balance - withdrawal.Amount, Version = state.Version + 1 },
                _ => state
            });

    public static EventSourcingResult ExecuteImperative(string streamId, int depositAmount)
    {
        var history = SeedHistory(streamId).ToList();
        var before = ProjectImperative(history);

        if (!before.Opened)
        {
            history.Add(new AccountOpened(streamId));
        }

        history.Add(new FundsDeposited(depositAmount));
        var after = ProjectImperative(history);

        return new EventSourcingResult(streamId, before, after, before.Version, after.Version);
    }

    public static EventSourcingResult ExecuteCSharpPipeline(string streamId, int depositAmount)
    {
        var history = SeedHistory(streamId);
        var before = ProjectCSharpPipeline(history);

        var withOpen = before.Opened
            ? history
            : history.Add(new AccountOpened(streamId));

        var afterHistory = withOpen.Add(new FundsDeposited(depositAmount));
        var after = ProjectCSharpPipeline(afterHistory);

        return new EventSourcingResult(streamId, before, after, before.Version, after.Version);
    }

    public static EventSourcingResult ExecuteLanguageExtPipeline(string streamId, int depositAmount)
    {
        var history = SeedHistory(streamId);
        var before = ProjectLanguageExt(history);

        var withOpen = before.Opened
            ? history
            : history.Add(new AccountOpened(streamId));

        var afterHistory = withOpen.Add(new FundsDeposited(depositAmount));
        var after = ProjectLanguageExt(afterHistory);

        return new EventSourcingResult(streamId, before, after, before.Version, after.Version);
    }

    public static string FormatSummary(EventSourcingResult result) =>
        $"Stream={result.StreamId}, before={result.Before.Balance}, after={result.After.Balance}, delta={result.Delta}, events={result.EventCountBefore}->{result.EventCountAfter}";
}
