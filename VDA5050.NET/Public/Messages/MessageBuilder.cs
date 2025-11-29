using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Public.Messages;

public sealed class MessageBuilder
{
    public Order.Order BuildOrderMessage(RobotOrder robotOrder)
    {
        
        return new Order.Order()
        {
            
        };
    }
}