using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Messages.Topics;
using VDA5050.NET.Public.Messages;
using VDA5050.NET.Public.Messages.Connection;
using VDA5050.NET.Public.Messages.Factsheet;
using VDA5050.NET.Public.Messages.State;
using VDA5050.NET.Public.Messages.Visualization;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public class RobotMessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<IMessageDispatcher> _logger;

    public RobotMessageDispatcher(
        IServiceProvider serviceProvider,
        ILogger<IMessageDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task DispatchMessage(string topic, string message)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var robotRepository = scope.ServiceProvider.GetRequiredService<IOperationalRobotRepository>();
        
        var robot = await robotRepository.GetRobotForTopic(topic);
        if (robot is null)
        {
            return;
        }

        var topicType = TopicTypeMapper.Decode(topic);
        if (topicType is null)
        {
            _logger.LogWarning("Unknown topic type: {topic}", topic);
        }

        switch (topicType)
        {
            case TopicType.State:
                var stateMessage = message.FromJson<State>();
                if (stateMessage is null)
                {
                    _logger.LogError("Could not deserialize state message for robot {robotId}", robot.SerialNumber);
                    break;
                }
                robot.OnStateMessage(stateMessage);
                break;
            case TopicType.Connection:
                var connectionMessage = message.FromJson<Connection>();
                if (connectionMessage is null)
                {
                    _logger.LogError("Could not deserialize connection message for robot {robotId}", robot.SerialNumber);
                    break;
                }
                robot.OnConnectionMessage(connectionMessage);
                break;
            case TopicType.Factsheet:
                var factsheetMessage = message.FromJson<Factsheet>();
                if (factsheetMessage is null)
                {
                    _logger.LogError("Could not deserialize factsheet message for robot {robotId}", robot.SerialNumber);
                    break;
                }
                robot.OnFactsheetMessage(factsheetMessage);
                break;
            case TopicType.Visualization:
                var visualizationMessage = message.FromJson<Visualization>();
                if (visualizationMessage is null)
                {
                    _logger.LogError("Could not deserialize factsheet message for robot {robotId}", robot.SerialNumber);
                    break;
                }
                robot.OnVisualizationMessage(visualizationMessage);
                break;
            default:
                _logger.LogError("Trying to dispatch message {message} for topic type: {topic}", message, topic);
                break;
        }
    }
}