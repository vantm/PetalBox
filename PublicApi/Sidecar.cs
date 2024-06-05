namespace PublicApi;

public static class Sidecar
{
    private static string? _url;
    private static string? _appId;

    public static string AppId()
    {
        _appId ??= Environment.GetEnvironmentVariable("APP_ID")!;
        return _appId;
    }

    public static string ResolveUrl()
    {
        if (_url == null)
        {
            var httpScheme = Environment.GetEnvironmentVariable("DAPR_HTTP_SCHEME") ?? "http";
            var httpHost = Environment.GetEnvironmentVariable("DAPR_HTTP_HOST") ?? "localhost";
            var httpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500";
            _url = $"{httpScheme}://{httpHost}:{httpPort}";
        }
        return _url;
    }

    public static string BindingUrl(string bindingName)
        => $"{ResolveUrl()}/v1.0/bindings/{bindingName}";
}
