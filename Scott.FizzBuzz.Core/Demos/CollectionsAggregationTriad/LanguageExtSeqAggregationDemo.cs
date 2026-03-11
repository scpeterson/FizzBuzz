using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CollectionsAggregationTriad;

public class LanguageExtSeqAggregationDemo : IDemo
{
    public const string DemoKey = "langext-seq-aggregation";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "collections", "aggregation"];

    public Either<string, Unit> Run(string? name, string? number) =>
        Aggregate(SampleOrders()).Map(_ => unit);

    private static Either<string, OrderSummary> Aggregate(Seq<Order> orders)
    {
        if (orders.IsEmpty)
            return Left<string, OrderSummary>("No orders to aggregate.");

        var count = orders.Count;
        var total = orders.Fold(0m, (sum, order) => sum + order.Amount);
        return Right<string, OrderSummary>(new OrderSummary(count, total));
    }

    private static Seq<Order> SampleOrders() =>
        Seq(
            new Order("Books", 40m),
            new Order("Games", 70m),
            new Order("Books", 25m));
}
