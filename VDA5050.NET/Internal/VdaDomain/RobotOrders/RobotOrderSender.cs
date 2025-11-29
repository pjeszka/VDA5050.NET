using System.Text;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed class RobotOrderSender : IRobotOrderSender
{
    private readonly IMqttConnection _mqttConnection;
    private readonly IOperationalRobotRepository _operationalRobotRepository;

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

        // create topic
        var topicBuilder = new StringBuilder();
        topicBuilder.Append(robot.TopicPrefix);
        topicBuilder.Append("/order");
        var topic = topicBuilder.ToString();
        // create payload
        // get order id

        _mqttConnection.PublishAsync(topic, )
    }

    public Task SendOrderUpdate(RobotOrderUpdate robotOrderUpdate)
    {
        throw new NotImplementedException();
    }

    public Task CancelOrder(OrderId orderId)
    {
        throw new NotImplementedException();
    }
}