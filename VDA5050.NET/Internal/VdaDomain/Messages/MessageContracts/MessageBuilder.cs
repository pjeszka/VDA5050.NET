using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.InstantAction;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;

internal sealed class MessageBuilder
{
    private readonly Dictionary<RobotSerialNumber, SentMessageHeaders> _sentMessageHeaders = new ();
    public OrderMessage BuildOrderMessage(
        OrderId orderId,
        RobotOrderRequest robotOrderRequest,
        string robotTopicPrefix,
        DateTime timeStamp,
        FactsheetProtocolInfo? factsheetProtocolInfo = null)
    {
        var headerId = GetNextOrderHeaderId(robotOrderRequest.RobotSerialNumber);
        
        var message = OrderMessage.CreateNewOrderMessage(
            headerId,
            robotTopicPrefix,
            timeStamp,
            orderId.Value,
            robotOrderRequest);
        
        RobotMessageValidator.ValidateOrderMessage(message, factsheetProtocolInfo);
        return message;
    }
    
    public OrderMessage BuildOrderUpdateMessage(
        RobotOrderUpdateRequest robotOrderUpdateRequest,
        string robotTopicPrefix,
        DateTime timeStamp,
        OrderUpdateId orderUpdateId,
        FactsheetProtocolInfo? factsheetProtocolInfo = null)
    {

        var headerId = GetNextOrderHeaderId(robotOrderUpdateRequest.RobotSerialNumber);
        
        var message = OrderMessage.CreateOrderUpdateMessage(
            headerId,
            robotTopicPrefix,
            timeStamp,
            orderUpdateId.Value,
            robotOrderUpdateRequest);
        
        RobotMessageValidator.ValidateOrderMessage(message, factsheetProtocolInfo);
        return message;
    }
    
    public InstantActionMessage BuildInstantActionMessage(
        RobotInstantActionRequest robotInstantActionRequest,
        string robotTopicPrefix,
        DateTime timeStamp,
        FactsheetProtocolInfo? factsheetProtocolInfo = null)
    {
        var headerId = GetNextInstantHeaderId(robotInstantActionRequest.RobotSerialNumber);
        var message = InstantActionMessage.Create(headerId, robotTopicPrefix, timeStamp, robotInstantActionRequest);
        RobotMessageValidator.ValidateInstantActionMessage(message, factsheetProtocolInfo);
        return message;
    }

    private uint GetNextOrderHeaderId(RobotSerialNumber robotSerialNumber)
    {
        uint headerId;
        if (_sentMessageHeaders.TryGetValue(robotSerialNumber, out var headers) is false)
        {
            _sentMessageHeaders.TryAdd(robotSerialNumber, new SentMessageHeaders());
            headerId = 0;
        }
        else
        {
            headerId = headers.GetNextOrderHeaderId();
        }
        return headerId;
    }
    
    private uint GetNextInstantHeaderId(RobotSerialNumber robotSerialNumber)
    {
        uint headerId;
        if (_sentMessageHeaders.TryGetValue(robotSerialNumber, out var headers) is false)
        {
            _sentMessageHeaders.TryAdd(robotSerialNumber, new SentMessageHeaders());
            headerId = 0;
        }
        else
        {
            headerId = headers.GetNextOrderHeaderId();
        }
        return headerId;
    }
}