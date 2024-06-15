using LanguageExt.Effects.Traits;

namespace Common;

public static class EitherExtensions
{
    public static Either<L, R> BindSelf<L, R>(this Either<L, R> either, Func<R, Either<L, R>> bind)
        => bindSelf(either, bind);

    public static async Task<Either<L, R>> BindSelf<L, R>(this Task<Either<L, R>> either, Func<R, Either<L, R>> bind)
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

    public static Aff<RT, T> MapAffAsync<RT, T>(this Eff<RT, Unit> unitEff, Func<CancellationToken, Task<T>> effect)
        where RT : struct, HasCancel<RT> => Aff<RT, T>(rt => effect.Invoke(rt.CancellationToken).ToValue());

    public static Task<Either<Error, T>> FlattenAsync<T>(this Task<Either<Error, Option<T>>> either)
        => either.BindAsync(opt => opt.Match<Either<Error, T>>(x => x, () => AppErrors.NotFoundError));

    public static ValueTask<IResult> Flatten(this ValueTask<Fin<IResult>> fin)
        => fin.Map(f => f.Match(nomap, ErrorHelper.MapError));

    public static Either<L, R> Tap<L, R>(this Either<L, R> monad, Action<R> action)
        => monad.Map(r =>
        {
            action?.Invoke(r);
            return r;
        });

    public static Task<T> Tap<T>(this Task<T> monad, Action<T> action) where T : IEither
        => monad.Map((r) =>
        {
            action?.Invoke(r);
            return r;
        });

    public static Task<T> TapAsync<T>(this Task<T> monad, Func<T, ValueTask> action) where T : IEither
        => monad.MapAsync(async r =>
        {
            if (action != null)
            {
                await action.Invoke(r);
            }
            return r;
        });
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

    public static T nomap<T>(T value) => value;

    public static IResult toHttpResult<T>(Option<T> option) where T : notnull
        => option.Match(x => Results.Ok(x), () => Results.NotFound());

    public static IResult toHttpResult(Error error)
        => ErrorHelper.MapError(error);

    public static IResult toHttpResult<T>(Either<Error, Option<T>> either) where T : notnull
        => either.Match(toHttpResult<T>, toHttpResult);

    public static IResult toHttpResult<T>(Either<Error, T> either, Func<T, IResult> map) where T : notnull
        => either.Match(map, toHttpResult);

    public static Aff<Unit> tapAsync(Func<ValueTask> action)
        => Aff(() =>
        {
            action?.Invoke();
            return ValueTask.FromResult(unit);
        });

    public static Eff<Unit> tap(Action action)
        => Eff(() =>
        {
            action?.Invoke();
            return unit;
        });

    public static Eff<R> flattenEff<R>(Either<Error, Option<R>> either)
        => either.Match(o => o.Match(SuccessEff, () => FailEff<R>(AppErrors.NotFoundError)), FailEff<R>);

    public static Eff<R> flattenEff<R>(Either<Error, R> either) where R : notnull
        => either.Match(SuccessEff, FailEff<R>);
}