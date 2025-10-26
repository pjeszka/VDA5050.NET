using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Services;

public sealed class RobotSettingsBuilder
{
    private RobotSettings _settings;
    
    public RobotSettingsBuilder()
    {
        _settings = new RobotSettings();
    }
    
    public RobotSettingsBuilder WithVisualization()
    {
        _settings = _settings with { ShouldObserveVisualization = true };
        return this;
    }

    public RobotSettings Build(string robotTopicPrefix, RobotSerialNumber robotSerialNumber)
    {
        return _settings with { RobotTopicPrefix = robotTopicPrefix, RobotSerialNumber = robotSerialNumber };
    }
}