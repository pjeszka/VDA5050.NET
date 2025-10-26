using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VDA5050.NET.Internal.Messages;
using VDA5050.NET.Internal.Messages.Connection;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Public.DependencyInjection.Settings;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public sealed class RobotDiscoveryService : IInitialTopicsHandler
{
    private readonly ICollection<string> _robotDiscoveryTopicPatterns;
    private readonly IDiscoveredRobotRepository _accessibleRobotRepository;
    private readonly ILogger<RobotDiscoveryService> _logger;

    public RobotDiscoveryService(
        Vda5050MasterSettings masterSettings,
        IDiscoveredRobotRepository accessibleRobotRepository,
        ILogger<RobotDiscoveryService> logger)
    {
        _logger = logger;
        _accessibleRobotRepository = accessibleRobotRepository;
        _robotDiscoveryTopicPatterns = masterSettings
            .RobotDiscovery
            .TopicPrefixes
            .Select(PrepareTopicPattern)
            .ToList();
    }

    public ICollection<string> GetInitialTopics()
    {
        return _robotDiscoveryTopicPatterns;
    }

    public Task HandleInitialTopicMessage(string topic, string message)
    {
        if (_robotDiscoveryTopicPatterns.Any(x => MqttTopicMatcher.IsMatch(topic, x)))
        {
            var connectionMessage = message.FromJson<Connection>();
            if (connectionMessage is null)
            {
                _logger.LogError("Could not deserialize connection message for robot discovery.");
                return Task.CompletedTask;
            }

            var existingRobot =
                _accessibleRobotRepository.GetRobot(new RobotSerialNumber(connectionMessage.SerialNumber));
            if (existingRobot is null)
            {
                _logger.LogInformation(
                    "Discovered robot with serial number {serialNumber}",
                    connectionMessage.SerialNumber);
            }
            else
            {
                if (existingRobot.ConnectionState != connectionMessage.ConnectionState)
                {
                    _logger.LogInformation(
                        "Discoverable robot with serial number {serialNumber} changed connection state from {previousConnectionState} to {connectionState}",
                        connectionMessage.SerialNumber,
                        existingRobot.ConnectionState,
                        connectionMessage.ConnectionState);
                    
                }
            }
            
            _accessibleRobotRepository.AddOrUpdateRobot(
                new DiscoveredRobot(
                    new RobotSerialNumber(connectionMessage.SerialNumber),
                    connectionMessage.Version,
                    GetRobotTopicPrefix(topic),
                    connectionMessage.ConnectionState));
        }
        
        return Task.CompletedTask;
    }
    
    private string PrepareTopicPattern(string prefix)
    {
        if (prefix.EndsWith("/"))
        {
            return $"{prefix}+/connection";       
        }

        return $"{prefix}/+/connection";
    }
    
    private string GetRobotTopicPrefix(string topic)
    {
        var lastSlash = topic.LastIndexOf('/');
        var result = lastSlash >= 0 ? topic[..lastSlash] : topic;
        return result;
    }
}