namespace VDA5050.NET.Internal.MQTT;

internal interface IMessageDispatcher
{
    Task DispatchMessage(string topic, string message);
}
