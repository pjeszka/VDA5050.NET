using System.Collections.Concurrent;
using System.Text;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed class RobotOrderSender : IRobotOrderSender
{
    private readonly IMqttConnection _mqttConnection;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly ConcurrentDictionary<RobotSerialNumber, int> _orderHeaderIds = new();

    public RobotOrderSender(
        IMqttConnection mqttConnection,
        IOperationalRobotRepository operationalRobotRepository)
    {
        _mqttConnection = mqttConnection;
        _operationalRobotRepository = operationalRobotRepository;
    }

    public async Task<OrderId> SendOrder(RobotOrderRequest robotOrderRequest)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var robot = await _operationalRobotRepository.GetRobot(robotOrderRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderRequest.RobotSerialNumber);
        }
        
        var newOrderId = new OrderId(Guid.NewGuid().ToString());
        
        _orderHeaderIds.AddOrUpdate(
            robotOrderRequest.RobotSerialNumber,
            0,
            (_, oldVal) =>
                oldVal+1);
        _orderHeaderIds.TryGetValue(robotOrderRequest.RobotSerialNumber, out var currentOrderHeaderId);
        
        var topic = CreateOrderTopicForRobot(robot);


        
        var orderMessage = OrderMessage.CreateNewOrderMessage(
            currentOrderHeaderId,
            robot.TopicPrefix,
            DateTime.UtcNow,
            newOrderId.Value,
            robotOrderRequest);

        await _mqttConnection.PublishAsync(topic, orderMessage.ToJson());

        return newOrderId;
    }

    private static string CreateOrderTopicForRobot(OperationalRobot robot)
    {
        var topicBuilder = new StringBuilder();
        topicBuilder.Append(robot.TopicPrefix);
        topicBuilder.Append("/order");
        var topic = topicBuilder.ToString();
        return topic;
    }

    public async Task<OrderUpdateId> SendOrderUpdate(RobotOrderUpdateRequest robotOrderUpdateRequest)
    {
        if (_mqttConnection.IsConnected is false)
        {
            throw new InvalidOperationException("MQTT connection is not connected");
        }

        var robot = await _operationalRobotRepository.GetRobot(robotOrderUpdateRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderUpdateRequest.RobotSerialNumber);
        }

        if (_orderHeaderIds.TryGetValue(robotOrderUpdateRequest.RobotSerialNumber, out var currentRobotOrderHeaderId) is false)
        {
            throw new InvalidOperationException($"Robot {robotOrderUpdateRequest.RobotSerialNumber} has no active order");
        }

        // TODO check this withs state before sending in master
        if (currentRobotOrderState.OrderId != robotOrderUpdateRequest.OrderUpdate.OrderId)
        {
            throw new InvalidOperationException(
                $"Robot {robotOrderUpdateRequest.RobotSerialNumber} has active order with different id. Update for order with id: {robotOrderUpdateRequest.OrderUpdate.OrderId.Value} and pending action has id: {currentRobotOrderState.OrderId.Value}");
        }
        
        _orderHeaderIds.TryUpdate(
            robotOrderUpdateRequest.RobotSerialNumber,
            currentRobotOrderHeaderId + 1,
            currentRobotOrderHeaderId);
        
        _orderHeaderIds.TryGetValue(robotOrderUpdateRequest.RobotSerialNumber, out var currentOrderHeaderId);
        
        var topic = CreateOrderTopicForRobot(robot);
        
        var orderMessage = OrderMessage.CreateOrderUpdateMessage(
            currentOrderHeaderId,
            robot.TopicPrefix,
            DateTime.UtcNow,
            robotOrderUpdateRequest.OrderUpdate.,
            robotOrderUpdateRequest);

        await _mqttConnection.PublishAsync(topic, orderMessage.ToJson());
    }
}