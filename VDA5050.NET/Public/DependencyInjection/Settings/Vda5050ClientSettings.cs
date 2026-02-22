namespace VDA5050.NET.Public.DependencyInjection.Settings;

public class Vda5050ClientSettings
{
    public const string SectionName = "VDA5050";
    public MqttConnectionSettings Mqtt { get; set; } = new MqttConnectionSettings();
    public RobotSettings Robot { get; set; } = new RobotSettings();
    
}