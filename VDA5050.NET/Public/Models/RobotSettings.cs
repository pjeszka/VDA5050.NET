using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Public.Models;

public sealed record RobotSettings
{
    public RobotSerialNumber RobotSerialNumber { get; set; }
    public ICollection<TopicType> SubscribedTopics { get; set; }
    public bool ShouldObserveVisualization { get; set; }
}