namespace Common;

public static class EitherExtensions
{
    public static Either<L, R> BindSelf<L, R>(this Either<L, R> either, Func<R, Either<L, R>> bind)
        => bindSelf(either, bind);

    public static async Task<Either<L, R>> BindSelfAsync<L, R>(this Task<Either<L, R>> either, Func<R, Either<L, R>> bind)
        => bindSelf(await either, bind);

    public static Either<L, R> LeftWhen<L, R>(this Either<L, R> either, bool cond, Func<L> createLeft)
        => leftWhen(either, _ => cond, createLeft);

    public static Either<L, R> LeftWhen<L, R>(this Either<L, R> either, Func<R, bool> pred, Func<L> createLeft)
        => leftWhen(either, pred, createLeft);

    public static async Task<Either<L, R>> LeftWhenAsync<L, R>(this Task<Either<L, R>> either, Func<R, bool> pred, Func<L> createLeft)
        => leftWhen(await either, pred, createLeft);

    public static Either<L, Unit> ToUnit<L, R>(this Either<L, R> either)
        => toUnit(either);

    public static async Task<Either<L, Unit>> ToUnitAsync<L, R>(this Task<Either<L, R>> either)
        => toUnit(await either);

    public static Task<R2> Match<L, R, R2>(this Task<Either<L, R>> either, Func<R, R2> Some, Func<L, R2> None)
        => either.Map(x => x.Match(Some, None));
}

public static class Prelube
{
    public static Either<Error, T> EitherWithError<T>(T value)
        => Either<Error, T>.Right(value);

    public static Either<L, R> bindSelf<L, R>(Either<L, R> either, Func<R, Either<L, R>> fn)
        => bind(either, fn);

    public static Either<L, R> leftWhen<L, R>(Either<L, R> either, Func<R, bool> pred, Func<L> createLeft)
        => either.IsLeft ? either : bindSelf(either, x => pred(x) ? createLeft() : x);

    public static Either<L, Unit> toUnit<L, R>(Either<L, R> either)
        => bind<L, R, Unit>(either, _ => unit);
}