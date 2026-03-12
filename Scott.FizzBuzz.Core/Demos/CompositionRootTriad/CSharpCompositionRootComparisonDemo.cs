using Scott.FizzBuzz.Core.Demos.Shared;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CompositionRootTriad;

public class CSharpCompositionRootComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpCompositionRootComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpCompositionRootComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-composition-root";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "composition-root", "triad"];
    public string Description => "Plain C# composition root flow using local result types to sequence dependency-provided calculations.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var env = new InMemoryFunctionalDemoEnvironment();
            var tier = CompositionRootRules.NormalizeTier(name);
            var region = "us";
            var normalizedTier = tier is "standard" or "vip" or "employee" ? tier : "standard";

            var amountResult = ParseAmount(number)
                .Bind(amount => ResolveDiscountRate(env, normalizedTier)
                    .Bind(discount => ResolveTaxRate(env, region)
                        .Map(tax => CompositionRootRules.CalculateTotal(amount, discount, tax))));

            if (amountResult.IsSuccess)
            {
                _output.WriteLine($"Result: total = {amountResult.Value:0.00}");
            }
            else
            {
                _output.WriteLine($"Failed: {amountResult.Error}");
            }
        }, "C# Composition Root Comparison");

    private static DemoResult<decimal> ParseAmount(string? number) =>
        CompositionRootRules.TryParseAmount(number, out var amount, out var error)
            ? DemoResult<decimal>.Success(amount)
            : DemoResult<decimal>.Failure(error);

    private static DemoResult<decimal> ResolveDiscountRate(IFunctionalDemoEnvironment env, string tier) =>
        env.TryResolveDiscountRate(tier, out var rate, out var error)
            ? DemoResult<decimal>.Success(rate)
            : DemoResult<decimal>.Failure(error);

    private static DemoResult<decimal> ResolveTaxRate(IFunctionalDemoEnvironment env, string region) =>
        env.TryResolveTaxRate(region, out var rate, out var error)
            ? DemoResult<decimal>.Success(rate)
            : DemoResult<decimal>.Failure(error);

    private readonly record struct DemoResult<T>(bool IsSuccess, T Value, string? Error)
    {
        public static DemoResult<T> Success(T value) => new(true, value, null);
        public static DemoResult<T> Failure(string? error) => new(false, default!, error);
        public DemoResult<TNext> Bind<TNext>(Func<T, DemoResult<TNext>> next) =>
            IsSuccess ? next(Value) : DemoResult<TNext>.Failure(Error);
        public DemoResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess ? DemoResult<TNext>.Success(map(Value)) : DemoResult<TNext>.Failure(Error);
    }
}
