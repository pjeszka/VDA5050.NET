using MQTTnet.Protocol;
using VDA5050.NET.Public.Models.MQTT;

namespace VDA5050.NET.Internal.MQTT;

internal interface IMqttConnection
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
