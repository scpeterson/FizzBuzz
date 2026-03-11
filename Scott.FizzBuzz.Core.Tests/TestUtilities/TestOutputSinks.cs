using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core.Tests.TestUtilities;

public sealed class NullOutputSink : IOutput
{
    public void WriteLine(string message)
    {
    }
}

public sealed class RecordingOutputSink : IOutput
{
    public List<string> Messages { get; } = [];

    public void WriteLine(string message) => Messages.Add(message);
}

public sealed class RecordingStyledOutputSink : IStyledOutput
{
    public List<string> Messages { get; } = [];

    public List<ConsoleColor> Colors { get; } = [];

    public void WriteLine(string message) => Messages.Add(message);

    public void WithColor(ConsoleColor color, Action action)
    {
        Colors.Add(color);
        action();
    }
}
