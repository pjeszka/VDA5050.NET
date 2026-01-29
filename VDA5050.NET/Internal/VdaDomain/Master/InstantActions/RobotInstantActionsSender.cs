using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master.InstantActions;

internal sealed class RobotInstantActionsSender : IRobotInstantActionsSender
{
    private readonly IMqttConnection _mqttConnection;
    private readonly MessageBuilder _messageBuilder;
    private readonly ISystemClock _systemClock;

    public RobotInstantActionsSender(
        IMqttConnection mqttConnection,
        MessageBuilder messageBuilder,
        ISystemClock systemClock)
    {
        _mqttConnection = mqttConnection;
        _messageBuilder = messageBuilder;
        _systemClock = systemClock;
    }

    public async Task<ICollection<ActionId>> SendInstantAction(OperationalRobot robot, RobotInstantActionRequest request)
    {
        var instantActionMessage = _messageBuilder.BuildInstantActionMessage(
            request,
            robot.TopicPrefix,
            _systemClock.Now,
            robot.FactsheetProtocolInfo);
        
        await _mqttConnection.PublishAsync(robot.OrderTopic, instantActionMessage.ToJson());

        return instantActionMessage.Actions.Select(actions => new ActionId(actions.ActionId)).ToList();
    }
}