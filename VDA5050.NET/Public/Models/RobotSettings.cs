using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Public.Models;

public sealed record RobotSettings
{
    public string RobotTopicPrefix { get; set; }
    public RobotSerialNumber RobotSerialNumber { get; set; }
    public bool ShouldObserveVisualization { get; set; }
}