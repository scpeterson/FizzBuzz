namespace Scott.FizzBuzz.Core.Interfaces;

public interface IDemo
{
    /// <summary>
    /// A unique key/name used to identify this demo in the DemoRunner.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// A simple classification for display/filtering.
    /// </summary>
    string Category => "functional";

    /// <summary>
    /// Search/filter tags. Most demos are functional by default.
    /// </summary>
    IReadOnlyCollection<string> Tags => ["fp"];

    /// <summary>
    /// Short human-readable description shown in demo listings.
    /// </summary>
    string Description => string.Empty;
    
    /// <summary>
    /// Run the demo, given two optional string parameters (name and number).
    /// Returns DemoExecutionResult (Error = failure message, success otherwise).
    /// </summary>
    DemoExecutionResult Run(string? name, string? number);
}
