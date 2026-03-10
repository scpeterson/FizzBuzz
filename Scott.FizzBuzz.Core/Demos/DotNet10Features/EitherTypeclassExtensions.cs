using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DotNet10Features;

public static class EitherTypeclassExtensions
{
    extension<TLeft, TRight>(Either<TLeft, TRight> source)
    {
        public Either<TLeft, TResult> MapFp<TResult>(Func<TRight, TResult> map) =>
            source.Match(
                Left: left => Left<TLeft, TResult>(left),
                Right: right => Right<TLeft, TResult>(map(right)));

        public Either<TLeft, TResult> BindFp<TResult>(Func<TRight, Either<TLeft, TResult>> bind) =>
            source.Match(
                Left: left => Left<TLeft, TResult>(left),
                Right: bind);
    }
}
