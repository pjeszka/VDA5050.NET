using MQTTnet.Protocol;

namespace VDA5050.NET.Internal.MQTT;

public interface IMqttConnection
{
    bool IsConnected { get; }
    bool HasSubscriber(string topic);
    Task AddSubscription(string topic);
    Task RemoveSubscription(string topic);
    Task ConnectAsync();
    Task PublishAsync(
        string topic,
        string messagePayload,
        bool isRetained = false,
        MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);
    MqttConnectionDetailsDto GetConnectionDetails();
}
