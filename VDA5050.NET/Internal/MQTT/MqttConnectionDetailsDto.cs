namespace VDA5050.NET.Internal.MQTT;

public sealed record MqttConnectionDetailsDto(
    bool IsConnected,
    string? ConnectionError,
    DateTime? LastConnectedAt,
    ICollection<string> SubscribedTopics);
