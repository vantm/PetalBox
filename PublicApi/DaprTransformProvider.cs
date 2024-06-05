using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace PublicApi;

public partial class DaprTransformProvider : ITransformProvider
{
    public void Apply(TransformBuilderContext context)
    {
        if (context.Cluster?.Destinations is null ||
            context.Cluster?.Metadata is null)
        {
            return;
        }

        if (!context.Cluster.Metadata.TryGetValue("DaprAppId", out var appId))
        {
            return;
        }

        context.AddRequestHeader("dapr-app-id", appId);
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }
}
