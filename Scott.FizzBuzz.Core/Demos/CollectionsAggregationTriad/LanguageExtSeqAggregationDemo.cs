using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CollectionsAggregationTriad;

public class LanguageExtSeqAggregationDemo : IDemo
{
    private readonly IOutput _output;

    public LanguageExtSeqAggregationDemo() : this(new ConsoleOutput())
    {
    }

    public LanguageExtSeqAggregationDemo(IOutput output) => _output = output;

    public const string DemoKey = "langext-seq-aggregation";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "comparison", "collections", "aggregation"];
    public string Description => "LanguageExt Seq aggregation pipeline over immutable collections with declarative summary construction.";

    public DemoExecutionResult Run(string? name, string? number) =>
        FunctionalDemoOutput.Render(
            _output,
            "LanguageExt Collections + Aggregation",
            Aggregate(SampleOrders()),
            (output, result) =>
            {
                output.WriteLine($"Result: total = {result.Total:0.00}");
                output.WriteLine($"Orders aggregated: {result.Count}");
            });

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
