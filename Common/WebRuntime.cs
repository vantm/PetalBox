namespace Common;

public class WebRuntime(IHttpContextAccessor httpContextAccessor) : IDisposable
{
    public ValueTask<IResult> RunAff(Aff<Runtime, IResult> aff)
        => aff.Run(_runtime).Flatten();

    private bool _disposedValue;

    private readonly Runtime _runtime = Runtime.New(new(httpContextAccessor.HttpContext!));

    public Runtime Runtime => _runtime;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _runtime.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
