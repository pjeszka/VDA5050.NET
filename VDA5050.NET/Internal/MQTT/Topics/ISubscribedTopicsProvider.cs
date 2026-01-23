namespace VDA5050.NET.Internal.MQTT.Topics;

internal interface ISubscribedTopicsProvider
{
    ICollection<string> GetTopicsToSubscribe();
}