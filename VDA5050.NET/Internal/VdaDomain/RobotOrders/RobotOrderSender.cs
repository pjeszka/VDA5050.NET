using System.Collections.Concurrent;
using System.Text;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Messages;
using VDA5050.NET.Public.Messages.Order;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed class RobotOrderSender : IRobotOrderSender
{
    private readonly IMqttConnection _mqttConnection;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly ConcurrentDictionary<RobotSerialNumber, CurrentRobotOrderState> _orders = new();

    public RobotOrderSender(
        IMqttConnection mqttConnection,
        IOperationalRobotRepository operationalRobotRepository)
    {
        _mqttConnection = mqttConnection;
        _operationalRobotRepository = operationalRobotRepository;
    }

    public async Task<OrderId> SendOrder(RobotOrder robotOrder)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var robot = await _operationalRobotRepository.GetRobot(robotOrder.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrder.RobotSerialNumber);
        }
        
        var newOrderId = new OrderId(Guid.NewGuid().ToString());
        
        _orders.AddOrUpdate(
            robotOrder.RobotSerialNumber,
            new CurrentRobotOrderState(0, newOrderId, 0),
            (_, oldVal) =>
                new CurrentRobotOrderState(oldVal.HeaderId+1, newOrderId, 0));
        _orders.TryGetValue(robotOrder.RobotSerialNumber, out var currentOrderState);
        
        var topic = CreateOrderTopicForRobot(robot);


        var orderMessage = Order.CreateNewOrderMessage(
            currentOrderState!.HeaderId,
            robot.TopicPrefix,
            DateTime.UtcNow,
            currentOrderState.OrderId.Value,
            robotOrder);

        await _mqttConnection.PublishAsync(topic, orderMessage.ToJson());

        return new OrderId(orderMessage.OrderId);
    }

    private static string CreateOrderTopicForRobot(OperationalRobot robot)
    {
        var topicBuilder = new StringBuilder();
        topicBuilder.Append(robot.TopicPrefix);
        topicBuilder.Append("/order");
        var topic = topicBuilder.ToString();
        return topic;
    }

    public async Task SendOrderUpdate(RobotOrderUpdate robotOrderUpdate)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var robot = await _operationalRobotRepository.GetRobot(robotOrderUpdate.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderUpdate.RobotSerialNumber);
        }

        if (_orders.TryGetValue(robotOrderUpdate.RobotSerialNumber, out var currentRobotOrderState) is false)
        {
            throw new InvalidOperationException($"Robot {robotOrderUpdate.RobotSerialNumber} has no active order");
        }

        if (currentRobotOrderState.OrderId != robotOrderUpdate.OrderUpdate.OrderId)
        {
            throw new InvalidOperationException(
                $"Robot {robotOrderUpdate.RobotSerialNumber} has active order with different id. Update for order with id: {robotOrderUpdate.OrderUpdate.OrderId.Value} and pending action has id: {currentRobotOrderState.OrderId.Value}");
        }
        
        _orders.TryUpdate(
            robotOrderUpdate.RobotSerialNumber,
            new CurrentRobotOrderState(
                currentRobotOrderState.HeaderId+1,
                currentRobotOrderState.OrderId,
                currentRobotOrderState.OrderUpdateId+1),
            currentRobotOrderState);
        
        _orders.TryGetValue(robotOrderUpdate.RobotSerialNumber, out var currentOrderState);
        
        var topic = CreateOrderTopicForRobot(robot);
        
        var orderMessage = Order.CreateOrderUpdateMessage(
            currentOrderState!.HeaderId,
            robot.TopicPrefix,
            DateTime.UtcNow,
            currentOrderState.OrderUpdateId,
            robotOrderUpdate);

        await _mqttConnection.PublishAsync(topic, orderMessage.ToJson());
    }
}