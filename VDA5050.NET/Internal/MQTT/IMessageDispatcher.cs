namespace VDA5050.NET.Internal.MQTT;

public interface IMessageDispatcher
{
    Task DispatchMessage(string topic, string message);
}
