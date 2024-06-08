using LanguageExt.Effects.Traits;

namespace Common;

public readonly struct Runtime : HasCancel<Runtime>, HasServiceProvider, IDisposable
{
    private readonly WebRuntimeEnv _env;
    private readonly CancellationTokenSource _cts;

    private Runtime(WebRuntimeEnv env, CancellationTokenSource cts)
    {
        _env = env;
        _cts = cts;
    }

    public static Runtime New(WebRuntimeEnv env)
    {
        var cts = new CancellationTokenSource();
        env.HttpContext.RequestAborted.Register(() => cts.Cancel());
        return new Runtime(env, cts);
    }

    public void Dispose()
    {
        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
        _cts.Dispose();
    }

    public Runtime LocalCancel => New(_env);

    public CancellationToken CancellationToken => _cts.Token;

    public CancellationTokenSource CancellationTokenSource => _cts;

    public Eff<IServiceProvider> ServiceProvider => SuccessEff(_env.HttpContext.RequestServices);

    public Eff<T> RequiredService<T>() where T : notnull =>
        from sp in ServiceProvider
        from sv in SuccessEff(sp.GetRequiredService<T>())
        select sv;
}

public record WebRuntimeEnv(HttpContext HttpContext)
{
}
