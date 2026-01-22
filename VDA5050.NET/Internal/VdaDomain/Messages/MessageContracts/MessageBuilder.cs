using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.InstantAction;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;

internal sealed class MessageBuilder
{
    // TODO per robot keep track of header id on 
    private readonly Dictionary<RobotSerialNumber, SentMessageHeaders> _sentMessageHeaders = new ();
    public OrderMessage BuildOrderMessage(
        RobotOrderRequest robotOrderRequest,
        string robotTopicPrefix,
        DateTime timeStamp)
    {
        var headerId = GetNextOrderHeaderId(robotOrderRequest.RobotSerialNumber);

        var orderId = Guid.NewGuid().ToString();
        return OrderMessage.CreateNewOrderMessage(
            headerId,
            robotTopicPrefix,
            timeStamp,
            orderId,
            robotOrderRequest);
    }
    
    public OrderMessage BuildOrderUpdateMessage(
        RobotOrderUpdateRequest robotOrderUpdateRequest,
        string robotTopicPrefix,
        DateTime timeStamp,
        uint currentOrderUpdateId)
    {

        var headerId = GetNextOrderHeaderId(robotOrderUpdateRequest.RobotSerialNumber);
        
        return OrderMessage.CreateOrderUpdateMessage(
            headerId,
            robotTopicPrefix,
            timeStamp,
            ++currentOrderUpdateId,
            robotOrderUpdateRequest);
    }
    
    public InstantActionMessage BuildInstantActionMessage(
        RobotInstantActionRequest robotInstantActionRequest,
        string robotTopicPrefix,
        DateTime timeStamp)
    {
        var headerId = GetNextInstantHeaderId(robotInstantActionRequest.RobotSerialNumber);
        return InstantActionMessage.Create(headerId, robotTopicPrefix, timeStamp, robotInstantActionRequest);
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