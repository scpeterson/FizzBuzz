using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.AsyncEffTriad;

public class ImperativeAsyncWorkflowDemo : IDemo
{
    private readonly IOutput _output;

    public ImperativeAsyncWorkflowDemo() : this(new ConsoleOutput())
    {
    }

    public ImperativeAsyncWorkflowDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "imperative-async-workflow";

    public string Key => DemoKey;
    public string Category => "imperative";
    public IReadOnlyCollection<string> Tags => ["imperative", "comparison", "async", "effects"];
    public string Description => "Imperative equivalent: direct side effects with sync + async calls and exception handling.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            try
            {
                var value = int.Parse(number ?? "10");
                var doubled = Double(value);
                var finalValue = AddTenAsync(doubled).GetAwaiter().GetResult();
                _output.WriteLine($"Result: {finalValue}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed: {ex.Message}");
            }
        }, "Imperative Eff/Aff Equivalent Workflow");

    private static int Double(int value) => value * 2;

    private static async Task<int> AddTenAsync(int value)
    {
        await Task.Delay(10);
        return value + 10;
    }
}
