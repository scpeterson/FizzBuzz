using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Core;

public static class OutputUtilities
{
    // Backward-compatible overload for demos that still use direct console output.
    public static DemoExecutionResult ExecuteWithSpacing(Action action, string methodName) =>
        ExecuteWithSpacing(new ConsoleOutput(), action, methodName);

    // Backward-compatible overload for demos that still use direct console output.
    public static void PrintHeader(string functionName) =>
        PrintHeader(new ConsoleOutput(), functionName);

    // Execute the action and set the output color to green when supported.
    public static void ExecuteActionWithColor(IOutput output, Action action)
    {
        if (output is IStyledOutput styledOutput)
        {
            styledOutput.WithColor(ConsoleColor.Green, action);
            return;
        }

        action();
    }

    private static void PrintDivider(IOutput output)
    {
        WriteColoredLine(output, "************************************************************", ConsoleColor.Cyan);
    }

    public static void PrintHeader(IOutput output, string functionName)
    {
        PrintDivider(output);
        WriteColoredLine(output, $"Executing method: {functionName}", ConsoleColor.Yellow);
        PrintDivider(output);
    }
    
    public static DemoExecutionResult ExecuteWithSpacing(IOutput output, Action action, string methodName)
    {
        PrintHeader(output, methodName);
        ExecuteActionWithColor(output, action);
        PrintDivider(output);

        return DemoExecutionResult.Success();
    }

    private static void WriteColoredLine(IOutput output, string message, ConsoleColor color)
    {
        if (output is IStyledOutput styledOutput)
        {
            styledOutput.WithColor(color, () => output.WriteLine(message));
            return;
        }

        output.WriteLine(message);
    }
}
