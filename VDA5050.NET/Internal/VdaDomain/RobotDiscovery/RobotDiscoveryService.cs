using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.Messages.Connection;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Public.DependencyInjection.Settings;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public sealed class RobotDiscoveryService : IInitialTopicsHandler
{
    private readonly ICollection<string> _robotDiscoveryPrefixes;
    private readonly IDiscoveredRobotRepository _accessibleRobotRepository;
    private readonly ILogger<RobotDiscoveryService> _logger;

    public RobotDiscoveryService(
        Vda5050MasterSettings masterSettings,
        IDiscoveredRobotRepository accessibleRobotRepository,
        ILogger<RobotDiscoveryService> logger)
    {
        _logger = logger;
        _accessibleRobotRepository = accessibleRobotRepository;
        _robotDiscoveryPrefixes = masterSettings.RobotDiscovery.TopicPrefixes;
    }

    public ICollection<string> GetInitialTopics()
    {
        return _robotDiscoveryPrefixes;
    }

    public Task HandleInitialTopicMessage(string topic, string message)
    {
        if (topic.Contains("connection"))
        {
            var connectionMessage = JsonSerializer.Deserialize<Connection>(message);
            if (connectionMessage is null)
            {
                _logger.LogError("Could not deserialize connection message for robot discovery.");
                return Task.CompletedTask;
            }
            
            _accessibleRobotRepository.AddRobot(
                new DiscoveredRobot(
                    new RobotSerialNumber(connectionMessage.SerialNumber),
                    connectionMessage.Version,
                    connectionMessage.ConnectionState));
        }
        
        return Task.CompletedTask;
    }
}