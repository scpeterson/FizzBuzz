namespace Scott.FizzBuzz.Core;

public readonly record struct DemoExecutionResult(bool IsSuccess, string? Error)
{
    public static DemoExecutionResult Success() => new(true, null);

    public static DemoExecutionResult Failure(string error) => new(false, error);
}
