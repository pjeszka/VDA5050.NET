
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

    public async Task<OrderId> SendOrder(OperationalRobot robot, RobotOrderRequest robotOrderRequest)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var orderMessage = _messageBuilder.BuildOrderMessage(
            robotOrderRequest,
            robot.TopicPrefix,
            _systemClock.Now);

        await _mqttConnection.PublishAsync(robot.OrderTopic, orderMessage.ToJson());

        return new OrderId(orderMessage.OrderId);
    }

    public async Task<OrderUpdateId> SendOrderUpdate(OperationalRobot robot, RobotOrderUpdateRequest robotOrderUpdateRequest)
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

        var currentOrderUpdateId = robot.OrderState?.OrderUpdateId?.Value ?? 0;
        
        var orderMessage = _messageBuilder.BuildOrderUpdateMessage(
            robotOrderUpdateRequest,
            robot.TopicPrefix,
            _systemClock.Now,
            currentOrderUpdateId);

        await _mqttConnection.PublishAsync(robot.OrderTopic, orderMessage.ToJson());
        
        return new OrderUpdateId(orderMessage.OrderUpdateId);
    }
}