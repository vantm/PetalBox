using Dapr.Client;
using Microsoft.Extensions.Options;

namespace Common.Implements;

public class DaprServiceBus(DaprClient dapr, IOptions<CommonOptions> options) : IServiceBus
{
    public Task PublishAsync(object evt)
    {
        var topicName = options.Value.DomainEventTopicName;
        var pubsubName = options.Value.PubsubName;
        return dapr.PublishEventAsync(pubsubName, topicName, data: evt);
    }
}

