using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CollectionsAggregationTriad;

public class ImperativeCollectionsAggregationDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeCollectionsAggregationDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeCollectionsAggregationDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-collections-aggregation";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "collections", "aggregation"];
    public string Description => "Imperative collection aggregation with a mutable dictionary and explicit update logic.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var totals = new Dictionary<string, decimal>();
            foreach (var order in SampleOrders())
            {
                if (!totals.ContainsKey(order.Category))
                    totals[order.Category] = 0m;

                totals[order.Category] += order.Amount;
            }

            foreach (var entry in totals)
                _output.WriteLine($"{entry.Key}: {entry.Value}");
        }, "Imperative Collections + Aggregation");

    private static IEnumerable<Order> SampleOrders() =>
    [
        new Order("Books", 40m),
        new Order("Games", 70m),
        new Order("Books", 25m)
    ];
}
