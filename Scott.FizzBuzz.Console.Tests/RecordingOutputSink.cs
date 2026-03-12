using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Console.Tests;

public sealed class RecordingOutputSink : IOutput
{
    public List<string> Lines { get; } = [];

    public void WriteLine(string message) => Lines.Add(message);
}
