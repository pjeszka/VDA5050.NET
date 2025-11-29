using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Public.Messages.Order;

public class Order : Header
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = default!;

    [JsonPropertyName("orderUpdateId")]
    public uint OrderUpdateId { get; set; }

    [JsonPropertyName("zoneSetId")]
    public string? ZoneSetId { get; set; }

    [JsonPropertyName("nodes")]
    public List<Node> Nodes { get; set; } = new();

    [JsonPropertyName("edges")]
    public List<Edge> Edges { get; set; } = new();

    public static Order CreateNewOrderMessage(
        int headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        string orderId,
        RobotOrder robotOrder)
    { 
        var order = new Order();
        order.FillHeader(headerId, robotTopicPrefix, timeStamp);
        order.OrderId = orderId;
        order.OrderUpdateId = 0;
        order.ZoneSetId = robotOrder.Order.ZoneSetId;
        order.Nodes = robotOrder.Order.Nodes
            .Select((x, index) => Node.CreateNodeMessage(x, (uint)index))
            .ToList();
        order.Edges = robotOrder.Order.Edges
            .Select((x, index) => Edge.CreateEdgeMessage(x, (uint)index))
            .ToList();
        
        
        return order;
    }
    
    public static Order CreateOrderUpdateMessage(
        int headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        uint orderUpdateId,
        RobotOrderUpdate robotOrderUpdate)
    {
        var order = new Order();
        order.FillHeader(headerId, robotTopicPrefix, timeStamp);
        order.OrderId = robotOrderUpdate.OrderUpdate.OrderId.Value;
        order.OrderUpdateId = orderUpdateId;
        order.ZoneSetId = robotOrderUpdate.OrderUpdate.ZoneSetId;
        order.Nodes = robotOrderUpdate.OrderUpdate.Nodes
            .Select((x, index) => Node.CreateNodeMessage(x, (uint)index))
            .ToList();
        order.Edges = robotOrderUpdate.OrderUpdate.Edges
            .Select((x, index) => Edge.CreateEdgeMessage(x, (uint)index))
            .ToList();
        
        
        return order;
    }
}
