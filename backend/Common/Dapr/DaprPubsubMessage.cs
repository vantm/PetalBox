namespace Common.Dapr;

public record DaprPubsubMessage<T>(T Data);
