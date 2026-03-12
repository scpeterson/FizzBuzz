using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.AsyncEffTriad;

public class CSharpAsyncCompositionDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpAsyncCompositionDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpAsyncCompositionDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-async-composition";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "async", "effects"];
    public string Description => "C# functional equivalent: plain result type with async composition.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            // The C# variant keeps async steps explicit with Task + a local result
            // type so learners can see the shape before the LanguageExt Aff/Eff version.
            var result = ComposeAsync(number ?? "10").GetAwaiter().GetResult();
            _output.WriteLine(result.IsSuccess
                ? $"Result: {result.Value}"
                : $"Failed: {result.Error}");
        }, "C# Eff/Aff Equivalent Workflow");

    private static async Task<AsyncResult<int>> ComposeAsync(string input)
    {
        var parsed = Parse(input);
        var doubled = await parsed.BindAsync(DoubleAsync);
        return await doubled.BindAsync(AddTenAsync);
    }

    private static AsyncResult<int> Parse(string input) =>
        int.TryParse(input, out var parsed)
            ? AsyncResult<int>.Success(parsed)
            : AsyncResult<int>.Failure("Input must be an integer.");

    private static async Task<AsyncResult<int>> DoubleAsync(int value)
    {
        await Task.Delay(10);
        return AsyncResult<int>.Success(value * 2);
    }

    private static async Task<AsyncResult<int>> AddTenAsync(int value)
    {
        await Task.Delay(10);
        return AsyncResult<int>.Success(value + 10);
    }

    private readonly record struct AsyncResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static AsyncResult<T> Success(T value) => new(true, value, null);
        public static AsyncResult<T> Failure(string error) => new(false, default!, error);

        public async Task<AsyncResult<TNext>> BindAsync<TNext>(Func<T, Task<AsyncResult<TNext>>> next) =>
            IsSuccess
                ? await next(Value)
                : AsyncResult<TNext>.Failure(Error ?? "Async pipeline failed.");
    }
}
