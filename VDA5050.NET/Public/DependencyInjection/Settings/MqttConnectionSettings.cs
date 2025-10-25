namespace VDA5050.NET.Public.DependencyInjection.Settings;

public sealed record MqttConnectionSettings
{
    public string ClientId { get; set; } = string.Empty;
    public int ReconnectionPeriodInMs { get; set; } = 1000;
    public string BrokerAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 1883;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
