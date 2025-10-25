namespace VDA5050.NET.Public.DependencyInjection.Settings;

public class Vda5050MasterSettings
{
    public const string SectionName = "VDA5050";
    public MqttConnectionSettings Mqtt { get; set; } = new MqttConnectionSettings();
    public RobotDiscoverySettings RobotDiscovery { get; set; } = new RobotDiscoverySettings();
}