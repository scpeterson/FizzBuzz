using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CollectionsAggregationTriad;

public class CSharpCollectionsAggregationDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpCollectionsAggregationDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpCollectionsAggregationDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-collections-aggregation";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "collections", "aggregation"];
    public string Description => "Plain C# aggregation pipeline using LINQ grouping and projection over immutable intermediate shapes.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var totals = SampleOrders()
                .GroupBy(order => order.Category)
                .Select(group => new { Category = group.Key, Total = group.Sum(order => order.Amount) });

            foreach (var row in totals)
                _output.WriteLine($"{row.Category}: {row.Total}");
        }, "C# Collections + Aggregation");

    private static IEnumerable<Order> SampleOrders() =>
    [
        new Order("Books", 40m),
        new Order("Games", 70m),
        new Order("Books", 25m)
    ];
}
