namespace VDA5050.NET.Internal.MQTT.Topics;

public interface ISubscribedTopicsProvider
{
    ICollection<string> GetTopicsToSubscribe();
}