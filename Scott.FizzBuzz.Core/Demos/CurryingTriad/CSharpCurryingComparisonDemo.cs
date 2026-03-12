using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.CurryingTriad;

public class CSharpCurryingComparisonDemo : IDemo
{
    private readonly IOutput _output;

    public CSharpCurryingComparisonDemo() : this(new ConsoleOutput())
    {
    }

    public CSharpCurryingComparisonDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "csharp-currying-comparison";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "comparison", "currying", "triad"];
    public string Description => "Plain C# currying and partial-application flow that removes repeated parameter threading.";

    public DemoExecutionResult Run(string? name, string? number) =>
        ExecuteWithSpacing(_output, () =>
        {
            var result = ParseBaseAmount(number)
                .Bind(amount => ResolveRates(name).Map(rates => (amount, rates)));

            if (!result.IsSuccess)
            {
                _output.WriteLine($"Failed: {result.Error}");
                return;
            }

            var amount = result.Value.amount;
            var rates = result.Value.rates;

            var applyBase = CurryingTriadRules.CurriedTotal(amount);
            var applyDiscount = applyBase(rates.DiscountRate);
            var total = applyDiscount(rates.TaxRate);

            _output.WriteLine($"Result: total = {total:0.00}");
        }, "C# Currying Comparison");

    private static DemoResult<decimal> ParseBaseAmount(string? number) =>
        CurryingTriadRules.TryParseBaseAmount(number, out var amount, out var error)
            ? DemoResult<decimal>.Success(amount)
            : DemoResult<decimal>.Failure(error);

    private static DemoResult<(decimal DiscountRate, decimal TaxRate)> ResolveRates(string? name) =>
        CurryingTriadRules.TryResolveRates(name, out var rates, out var error)
            ? DemoResult<(decimal DiscountRate, decimal TaxRate)>.Success(rates)
            : DemoResult<(decimal DiscountRate, decimal TaxRate)>.Failure(error);

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
