// -----------------------------------------------------------------------------
// OtherMonadsDemo.cs
// 
// This demo class illustrates six common monads in LanguageExt for C# developers:
// 1) Either<TLeft,TRight> via LINQ query syntax (fail-fast binding)
// 2) Either<TLeft,TRight> for simple fail-fast validation
// 3) Reader<Env,A> for dependency/configuration injection
// 4) Writer<W,A> to accumulate logs alongside a computation
// 5) Sequence/Traverse to invert collections of Option/Either into a single monad
// 6) Aff<T> (async monad) for asynchronous workflows using ValueTask
// -----------------------------------------------------------------------------

using LanguageExt;
using LanguageExt.ClassInstances;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;
using static Scott.FizzBuzz.Core.OutputUtilities;

namespace Scott.FizzBuzz.Core.Demos.MonadFoundations;

public class OtherMonadsDemo : IDemo
{
    private readonly IOutput _output;

    public OtherMonadsDemo() : this(new ConsoleOutput())
    {
    }

    public OtherMonadsDemo(IOutput output)
    {
        _output = output;
    }

    public string Key => "other-monads";
    public string Category => "functional";
    public IReadOnlyCollection<string> Tags => ["fp", "languageext", "monads", "advanced"];

    public Either<string, Unit> Run(string? _1, string? _2) =>
        ExecuteWithSpacing(_output, () =>
            {
                // 1) LINQ‑style monadic comprehension with Either
                _output.WriteLine("=== LINQ Comprehension (Either) ===");
                Either<string, int> SumTwo(string a, string b) =>
                    from x in TryParseInt(a)
                    from y in TryParseInt(b)
                    select x + y;

                var sumResult = SumTwo("3", "4");
                sumResult.Match(
                    Right: v => _output.WriteLine($"3 + 4 = {v}"),
                    Left: err => _output.WriteLine(err)
                );
                
                _output.WriteLine(string.Empty);
                
                // 2) Validation (Either) - fail-fast
                _output.WriteLine("=== Validation (Either) ===");
                var validationResult = OtherMonadsValidation.ValidateUser("", "short");
                validationResult.Match(
                    Right: u   => _output.WriteLine($"User OK: {u.Email}"),
                    Left: err  => _output.WriteLine(err)
                );
                
                // 3) Reader Monad
                _output.WriteLine("=== Reader Monad ===");
                var conn = ConnectionStringReader.Run(new Config("Server=.;Database=Test;"));
                conn.Match(
                    Succ: s1 => _output.WriteLine($"ConnStr: {s1}"),
                    Fail: err => _output.WriteLine(err.Message));
               
                
                // 4) Writer Monad
                _output.WriteLine("=== Writer Monad ===");
                var (writerResult, writerLogs) = ComputeLogged(5).Run();
                writerResult.Match(
                    i => _output.WriteLine($"Result: {i}"),
                    () => _output.WriteLine("Unknown error"));
                
                writerLogs.Iter(log => _output.WriteLine(log));
                
                // 5) Sequence/Traverse for collections of Option/Either
                _output.WriteLine("=== Sequence / Traverse ===");
                var maybes = new[] { Some(1), Some(2), None, Some(4) };
                var sequencedOpt = maybes.Sequence();
                _output.WriteLine(sequencedOpt.Match(
                    Some: seq => $"All present: {string.Join(',', seq)}",
                    None:  () => "One value was None"
                ));
                var rights = new[]
                {
                    Right<string,int>(10),
                    Left<string,int>("oops"),
                    Right<string,int>(30)
                };
                var sequencedEither = rights.Sequence();
                sequencedEither.Match(
                    Right: seq => _output.WriteLine($"Eithers OK: {string.Join(',', seq)}"),
                    Left: err => _output.WriteLine($"Failed: {err}")
                );
                
                // 6) Async / Task Monad via Aff
                _output.WriteLine("=== Async / Task Monad ===");
                // Use a ValueTask-based async method for Aff
                var aff = Aff<int>(GetValueAsync)
                    .Map(x => x * 2)
                    .Bind(x => Aff<int>(() => new ValueTask<int>(x + 1)));

                var final = aff.Run().AsTask().GetAwaiter().GetResult();
                _output.WriteLine($"Async result: {final}");
                
                
            },
            "LINQ Comprehension Demo");

    // Utility to parse ints into Either
    private static Either<string, int> TryParseInt(string s) =>
        int.TryParse(s, out var n)
            ? Right<string, int>(n)
            : Left<string, int>($"'{s}' is not a number.");
    
    private record Config(string ConnectionString);
    
    private static readonly Reader<Config, string> ConnectionStringReader =
        Reader<Config, string>(cfg => cfg.ConnectionString);

    // Writer helper moved to class scope
    private static Writer<MSeq<string>, Seq<string>, int> ComputeLogged(int x) =>
        from a in Writer<MSeq<string>, Seq<string>, int>(x * 2, Seq1($"Doubled {x} -> {x * 2}"))
        from b in Writer<MSeq<string>, Seq<string>, int>(a + 3, Seq1($"Added 3 -> {a + 3}"))
        select b;
    
    // Async helper moved to class scope
    private static ValueTask<int> GetValueAsync() => new ValueTask<int>(42);
}
