namespace VDA5050.NET.MQTT;

public interface IMqttConnection
{
    bool IsConnected { get; }
    bool HasSubscriber(string topic);
    Task AddSubscription(string topic);
    Task RemoveSubscription(string topic);
    Task ConnectAsync();
    MqttConnectionDetailsDto GetConnectionDetails();
}
