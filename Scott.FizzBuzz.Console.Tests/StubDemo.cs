using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Console.Tests;

public sealed class StubDemo(
    string key,
    Func<string?, string?, DemoExecutionResult> runFunc,
    string category = "functional",
    IReadOnlyCollection<string>? tags = null,
    string description = "") : IDemo
{
    public string Key { get; } = key;
    public string Category { get; } = category;
    public IReadOnlyCollection<string> Tags { get; } = tags ?? ["fp"];
    public string Description { get; } = description;

    public DemoExecutionResult Run(string? name, string? number) => runFunc(name, number);
}
