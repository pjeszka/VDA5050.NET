namespace VDA5050.NET.Public.DependencyInjection.Settings;

public sealed class RobotDiscoverySettings
{
    public ICollection<string> TopicPrefixes { get; set; } = new List<string>();
}