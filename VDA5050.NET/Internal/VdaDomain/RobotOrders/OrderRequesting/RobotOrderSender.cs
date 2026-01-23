
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

internal sealed class RobotOrderSender : IRobotOrderSender
{
    private readonly IMqttConnection _mqttConnection;
    private readonly MessageBuilder _messageBuilder;
    private readonly ISystemClock _systemClock;

    public RobotOrderSender(
        IMqttConnection mqttConnection,
        MessageBuilder messageBuilder,
        ISystemClock systemClock)
    {
        _mqttConnection = mqttConnection;
        _messageBuilder = messageBuilder;
        _systemClock = systemClock;
    }

    public Task SendOrder(OperationalRobot robot, RobotOrderRequest robotOrderRequest, OrderId orderId)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var orderMessage = _messageBuilder.BuildOrderMessage(
            orderId,
            robotOrderRequest,
            robot.TopicPrefix,
            _systemClock.Now,
            robot.FactsheetProtocolInfo);

        return _mqttConnection.PublishAsync(robot.OrderTopic, orderMessage.ToJson());
    }

    public Task SendOrderUpdate(OperationalRobot robot, RobotOrderUpdateRequest robotOrderUpdateRequest, OrderUpdateId orderUpdateId)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        if (robot.OrderState?.OrderId is null)
        {
            throw new InvalidOperationException($"Robot {robotOrderUpdateRequest.RobotSerialNumber} has no active order");
        }

        if (robot.OrderState.OrderId != robotOrderUpdateRequest.Request.OrderId)
        {
            throw new InvalidOperationException(
                $"Robot {robotOrderUpdateRequest.RobotSerialNumber} has active order with different id. Update for order with id: {robotOrderUpdateRequest.Request.OrderId!.Value} and pending action has id: {robot.OrderState.OrderId.Value}");
        }
        
        var orderMessage = _messageBuilder.BuildOrderUpdateMessage(
            robotOrderUpdateRequest,
            robot.TopicPrefix,
            _systemClock.Now,
            orderUpdateId,
            robot.FactsheetProtocolInfo);

        return _mqttConnection.PublishAsync(robot.OrderTopic, orderMessage.ToJson());
    }
}