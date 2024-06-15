using Carter;
using Dapr;
using System.Text.Json;

namespace MessageApi.Notifications;

public class NotificationPubsubModule() : CarterModule("/notify-pubsub")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("product-created", (
            DaprData<ProductCreatedPayload> payload, ILogger<NotificationPubsubModule> logger) =>
        {
            logger.LogInformation("A product has been created: {Json}",
                JsonSerializer.Serialize(payload.Data));
        });

        app.MapPost("notify-requested", (
            DaprData<SendingNotificationRequestPayload> payload, ILogger<NotificationPubsubModule> logger) =>
        {
            logger.LogInformation("A sending notification request has been received: {Json}",
                JsonSerializer.Serialize(payload.Data));
        });
    }
}

record DaprData<T>(T Data);
record ProductCreatedPayload(Guid Id, string Title);
record SendingNotificationRequestPayload(string Message);
