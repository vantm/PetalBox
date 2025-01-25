using System.Security.Claims;

namespace Common;

public interface IAppRuntime
{
    Eff<IServiceProvider> ServiceProvider { get; }
    Eff<CancellationToken> CancellationToken { get; }
    Eff<Option<ClaimsPrincipal>> User { get; }
    Eff<T> RequiredService<T>() where T : notnull;
    Eff<Option<T>> OptionalService<T>() where T : notnull;
}