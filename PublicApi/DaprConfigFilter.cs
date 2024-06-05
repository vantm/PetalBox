using Yarp.ReverseProxy.Configuration;

namespace PublicApi;

public class DaprConfigFilter(ILogger<DaprConfigFilter> logger) : IProxyConfigFilter
{
    public ValueTask<ClusterConfig> ConfigureClusterAsync(
        ClusterConfig cluster, CancellationToken cancel)
    {
        if (cluster.Destinations is null)
        {
            return ValueTask.FromResult(cluster);
        }

        var sidecarUrl = Sidecar.ResolveUrl();

        var newDest = new Dictionary<string, DestinationConfig>()
        {
            ["daprSidecar"] = new() { Address = sidecarUrl }
        };

        logger.LogInformation("Adding daprSidecar address {url} for the cluster {name}",
            sidecarUrl, cluster.ClusterId);

        return ValueTask.FromResult(cluster with
        {
            Destinations = newDest
        });
    }

    public ValueTask<RouteConfig> ConfigureRouteAsync(
        RouteConfig route, ClusterConfig? cluster, CancellationToken cancel)
    {
        return ValueTask.FromResult(route);
    }
}
