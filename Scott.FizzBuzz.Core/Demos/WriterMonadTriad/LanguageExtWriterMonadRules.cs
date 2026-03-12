using LanguageExt;
using LanguageExt.ClassInstances;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.WriterMonadTriad;

public static class LanguageExtWriterMonadRules
{
    public static Either<string, Seq<int>> ResolveOps(string? name) =>
        WriterMonadRules.TryResolveOps(name, out var ops, out var error)
            ? Right<string, Seq<int>>(toSeq(ops!))
            : Left<string, Seq<int>>(error ?? "Unknown writer profile. Use standard or aggressive.");

    public static Either<string, int> ParseStart(string? number) =>
        WriterMonadRules.TryParseStart(number, out var start, out var error)
            ? Right<string, int>(start)
            : Left<string, int>(error ?? "Start value must be numeric.");

    public static Writer<MSeq<string>, Seq<string>, int> Step(int state, int op)
    {
        var next = WriterMonadRules.Step(state, op);
        return Writer<MSeq<string>, Seq<string>, int>(next.NextState, Seq1(next.LogEntry));
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
