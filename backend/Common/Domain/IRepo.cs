namespace Common.Domain;

public interface IRepo<T, TId, RT>
    where T : AggregateRoot<TId>
    where RT : IAppRuntime
{
    Eff<RT, Option<T>> of(TId id);
    Eff<RT, Unit> insert(T order);
    Eff<RT, Unit> update(T order);
}
