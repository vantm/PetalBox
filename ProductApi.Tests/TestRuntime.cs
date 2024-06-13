using LanguageExt.Effects.Traits;

namespace ProductApi.Tests;

public struct TestRuntime : HasCancel<TestRuntime>, HasServiceProvider
{
    private readonly HasCancel<TestRuntime> _hasCancel;
    private readonly HasServiceProvider _hasServiceProvider;

    public static TestRuntime New(
        HasCancel<TestRuntime> hasCancel,
        HasServiceProvider hasServiceProvider)
    {
        return new(hasCancel, hasServiceProvider);
    }

    private TestRuntime(
        HasCancel<TestRuntime> hasCancel,
        HasServiceProvider hasServiceProvider)
    {
        _hasCancel = hasCancel;
        _hasServiceProvider = hasServiceProvider;
    }

    public TestRuntime LocalCancel => New(_hasCancel, _hasServiceProvider);
    public Eff<IServiceProvider> ServiceProvider => _hasServiceProvider.ServiceProvider;
    public CancellationToken CancellationToken => _hasCancel.CancellationToken;
    public CancellationTokenSource CancellationTokenSource => _hasCancel.CancellationTokenSource;

    public Eff<T> RequiredService<T>() where T : notnull
        => _hasServiceProvider.RequiredService<T>();
}
