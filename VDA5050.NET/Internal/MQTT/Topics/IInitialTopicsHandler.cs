namespace VDA5050.NET.Internal.MQTT.Topics;

internal interface IInitialTopicsHandler
{
    ICollection<string> GetInitialTopics();
    Task HandleInitialTopicMessage(string topic, string message);
}