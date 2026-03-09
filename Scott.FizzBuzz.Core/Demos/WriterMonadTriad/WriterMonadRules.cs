using LanguageExt;
using LanguageExt.ClassInstances;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public static class WriterMonadRules
{
    public static Either<string, Seq<int>> ResolveOps(string? name)
    {
        var key = string.IsNullOrWhiteSpace(name) ? "standard" : name.Trim().ToLowerInvariant();
        return key switch
        {
            "standard" => Right<string, Seq<int>>(Seq(2, 3, -1)),
            "aggressive" => Right<string, Seq<int>>(Seq(3, 4, -2, 5)),
            "scott" => Right<string, Seq<int>>(Seq(2, 2, -1)),
            _ => Left<string, Seq<int>>("Unknown writer profile. Use standard or aggressive.")
        };
    }

    public static Either<string, int> ParseStart(string? number) =>
        int.TryParse(number, out var start)
            ? Right<string, int>(start)
            : Left<string, int>("Start value must be numeric.");

    public static Writer<MSeq<string>, Seq<string>, int> Step(int state, int op)
    {
        var next = op >= 0 ? state + op : state - Math.Abs(op);
        var msg = op >= 0
            ? $"Added {op}, state={next}"
            : $"Subtracted {Math.Abs(op)}, state={next}";

        return Writer<MSeq<string>, Seq<string>, int>(next, Seq1(msg));
    }

    public static Writer<MSeq<string>, Seq<string>, int> RunProgram(int start, Seq<int> ops)
    {
        var program = Writer<MSeq<string>, Seq<string>, int>(start, Seq<string>());
        foreach (var op in ops)
        {
            program =
                from state in program
                from next in Step(state, op)
                select next;
        }

        return program;
    }
}
