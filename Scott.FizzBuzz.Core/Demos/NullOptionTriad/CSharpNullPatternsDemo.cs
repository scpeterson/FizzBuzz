using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.NullOptionTriad;

public class CSharpNullPatternsDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpNullPatternsDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpNullPatternsDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-null-patterns";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "null"];
    public string Description => "Plain C# null-handling pipeline using normalization, validation, and projection steps.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            // This mirrors an option-style flow in plain C#: normalize first,
            // then validate, then project to the final greeting.
            var message = Normalize(name)
                .Bind(RequireNonEmpty)
                .Map(ProjectGreeting)
                .Match(
                    onSuccess: greeting => greeting,
                    onFailure: error => error);

            _output.WriteLine(message);
        }, "C# Null Patterns");

    private static NameResult<string?> Normalize(string? raw) =>
        NameResult<string?>.Success(raw?.Trim());

    private static NameResult<string> RequireNonEmpty(string? value) =>
        value switch
        {
            null => NameResult<string>.Failure("Name missing."),
            { Length: 0 } => NameResult<string>.Failure("Name empty."),
            _ => NameResult<string>.Success(value)
        };

    private static string ProjectGreeting(string name) => $"Hello, {name}.";

    private readonly record struct NameResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static NameResult<T> Success(T value) => new(true, value, null);
        public static NameResult<T> Failure(string error) => new(false, default!, error);

        public NameResult<TNext> Bind<TNext>(Func<T, NameResult<TNext>> next) =>
            IsSuccess
                ? next(Value)
                : NameResult<TNext>.Failure(Error ?? "Name pipeline failed.");

        public NameResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess
                ? NameResult<TNext>.Success(map(Value))
                : NameResult<TNext>.Failure(Error ?? "Name pipeline failed.");

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure) =>
            IsSuccess
                ? onSuccess(Value)
                : onFailure(Error ?? "Name pipeline failed.");
    }
}
