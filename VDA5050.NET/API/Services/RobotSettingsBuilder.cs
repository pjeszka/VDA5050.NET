using VDA5050.NET.API.Models;
using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.API.Services;

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

    public RobotSettings Build(RobotNetworkName robotNetworkName)
    {
        return _settings with { RobotNetworkName = robotNetworkName };
    }
}