using Dapr.Client;
using Microsoft.Extensions.Options;

namespace Common.Implements;

public class DaprServiceBus(DaprClient dapr, IOptions<CommonOptions> options) : IServiceBus
{
    public async ValueTask PublishAsync(IDomainEvent evt)
    {
        var topicName = options.Value.DomainEventTopicName;
        var pubsubName = options.Value.PubsubName;
        await dapr.PublishEventAsync(pubsubName, topicName, data: evt);
    }
}

