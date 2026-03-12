using LanguageExt;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos;

public class SchrodingerDemo(IOutput output) : IDemo
{
    private readonly IOutput _output = output;

    public SchrodingerDemo() : this(new ConsoleOutput())
    {
    }

    public string Key => "demo-cat-monad";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "monads", "seq", "nondeterminism"];
    public string Description => "Legacy nondeterminism demo using LanguageExt sequences to model multiple possible cat states.";
    public DemoExecutionResult Run(string? _, string? __) =>
        ExecuteWithSpacing(_output, () =>
            {
                _output.WriteLine("Generating possible states for Schrödinger's cat...");

                // 1) A Seq<bool> is our non-determinism monad: true=alive, false=dead
                var possibleStates = Seq(true, false);

                // 2) Map each bool to a human-readable description
                var descriptions =
                    from alive in possibleStates
                    select alive
                        ? "Cat is alive"
                        : "Cat is dead";

                // 3) Iterate and print each possible world
                descriptions.Iter(_output.WriteLine);
            },
            "Schrodinger Cat Non-Determinism Demo");
}
