using System.Security.Claims;

namespace Common;

public readonly struct AppRuntime(HttpContext context) : IAppRuntime
{
    public Eff<IServiceProvider> ServiceProvider => SuccessEff(context.RequestServices);
    public Eff<CancellationToken> CancellationToken => SuccessEff(context.RequestAborted);

    public Eff<Option<ClaimsPrincipal>> User => SuccessEff(Optional(context.User));

    public Eff<Option<T>> OptionalService<T>() where T : notnull =>
        from sp in ServiceProvider
        from sv in liftEff(sp.GetService<T>)
        let ret = Optional(sv)
        select ret;

    public Eff<T> RequiredService<T>() where T : notnull =>
        from sp in ServiceProvider
        from sv in liftEff(sp.GetRequiredService<T>)
        select sv;
}
