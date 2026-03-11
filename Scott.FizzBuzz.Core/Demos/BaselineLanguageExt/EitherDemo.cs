using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using Scott.FizzBuzz.Core;
using LanguageExt.Common;
using System.Numerics;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.BaselineLanguageExt;

public class EitherDemo : IDemo
{
    private readonly IOutput _output;

    public EitherDemo() : this(new ConsoleOutput())
    {
    }

    public EitherDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "demo-either";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "either", "validation", "baseline"];
    public Either<string, Unit> Run(string? name, string? number)
    {
        // 1) Imperative validation
        PrintHeader(_output, "Imperative Validation");
        var imp = EitherValidation.ImperativeValidate(number);
        imp.Match(
            Right: val => _output.WriteLine($"[Imperative] Valid number: {val}"),
            Left: err => _output.WriteLine($"[Imperative] Error: {err}")
        );

        _output.WriteLine(string.Empty);
        
        // 2) Functional validation via small composable functions
        PrintHeader(_output, "Functional Validation (Either/Bind)");
        var func = EitherValidation.FunctionalValidate(number);
        func.Match(
            Right: val => _output.WriteLine($"[Functional] Valid number: {val}"),
            Left: err => _output.WriteLine($"[Functional] Error: {err}")
        );
        return unit;
    }

    // Func<int, Validation<Seq<string>, int>> CheckLessThan(int max) =>
    //     n => n < max
    //         ? Success<Seq<string>, int>(n)
    //         : Fail<Seq<string>, int>(Seq1($"Must be less than {max}"));
    
    Func<int, Validation<Seq<string>, int>> CheckGreaterThan(int min) =>
        n => n > min
            ? Success<Seq<string>, int>(n)
            : Fail<Seq<string>, int>(Seq1($"Must be greater than {min}"));
    
    public static Validation<Error, T?> ValidateGreaterThanZeroNullable<T>(Option<T?> value, string parameterName)
        where T : struct, INumber<T>
    {
        return value.Match(
            Some: v => !v.HasValue || v.Value > T.Zero
                ? Success<Error, T?>(v)
                : Fail<Error, T?>(Error.New($"The value '{v}' must be greater than zero for '{parameterName}'.")),
            None: () => Fail<Error, T?>(Error.New($"The value of '{parameterName}' cannot be null.")));
    }
}
