using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.MonadFoundations;

public class SchrodingersCatDemo : IDemo
{
    private readonly IOutput _output;

    public SchrodingersCatDemo() : this(new ConsoleOutput())
    {
    }

    public SchrodingersCatDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "demo-monad";

    public string Key => DemoKey;
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "monads", "state", "try"];
    
    private enum CatState { Alive, Dead }

    // box holds whether we've opened it and (once opened) the observed state
    private record Box(bool IsOpened, CatState? LastState = null);
    
    public Either<string, Unit> Run(string? _, string? __) =>
        ExecuteWithSpacing(_output, () =>
            {
                _output.WriteLine("🛸 Schrödinger’s Cat Monad Demo\n");

                // 1) Try<CatState>
                var tryObservation = Try<CatState>(() =>
                {
                    var rnd = new Random();
                    return rnd.NextDouble() < 0.5
                        ? CatState.Alive
                        : CatState.Dead;
                });

                tryObservation.Match(
                    Succ: state => _output.WriteLine($"[Try] Cat is {state}"),
                    Fail:  ex=> _output.WriteLine($"[Try] Observation failed: {ex.Message}")
                );

                _output.WriteLine(string.Empty);

                // 2) State<Box,CatState>
                var initialBox = new Box(IsOpened: false);

                var openBox = new State<Box, CatState>(box =>
                {
                    if (!box.IsOpened)
                    {
                        var rnd    = new Random();
                        var result = rnd.NextDouble() < 0.5
                            ? CatState.Alive
                            : CatState.Dead;

                        var nextBox = new Box(IsOpened: true, LastState: result);
                        return new ValueTuple<CatState, Box?, bool>(result, nextBox, false);
                    }

                    var stored = box.LastState ?? CatState.Dead;
                    //return (stored, box);
                    return new ValueTuple<CatState, Box?, bool>(stored, box, false);
                });

                var (observedState, finalBox) = openBox.Run(initialBox);
                var value = observedState.Match(
                    state => state.ToString(), 
                    () => "Unknown"
                    );
                _output.WriteLine($"[State] Observed cat: {value}, Box opened? {finalBox.IsOpened}");
            },
            "Schrödinger’s Cat Monad Demo"
        );
}
