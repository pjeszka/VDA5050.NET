namespace VDA5050.NET.MQTT.Settings;

public sealed record MqttConnectionSettings
{
    public const string SectionName = "ExternalTriggers:Sources:MQTT:Connection";
    public int ReconnectionPeriodInMs { get; set; }
    public string BrokerAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
