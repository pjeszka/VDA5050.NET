namespace VDA5050.NET.MQTT;

public interface IMessageDispatcher
{
    Task DispatchMessage(string topic, string message);
}
