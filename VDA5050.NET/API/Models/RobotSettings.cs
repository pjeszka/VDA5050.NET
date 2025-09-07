using VDA5050.NET.MQTT.Topics;
using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.API.Models;

public sealed record RobotSettings
{
    public RobotNetworkName RobotNetworkName { get; set; }
    public ICollection<TopicType> SubscribedTopics { get; set; }
}