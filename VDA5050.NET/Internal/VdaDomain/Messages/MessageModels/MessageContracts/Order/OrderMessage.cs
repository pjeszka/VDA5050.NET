using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;

internal class OrderMessage : Header
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = default!;

    [JsonPropertyName("orderUpdateId")]
    public uint OrderUpdateId { get; set; }

    [JsonPropertyName("zoneSetId")]
    public string? ZoneSetId { get; set; }

    [JsonPropertyName("nodes")]
    public List<NodeMessage> Nodes { get; set; } = new();

    [JsonPropertyName("edges")]
    public List<EdgeMessage> Edges { get; set; } = new();

    public static OrderMessage CreateNewOrderMessage(
        uint headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        string orderId,
        RobotOrderRequest robotOrderRequest)
    { 
        var order = new OrderMessage();
        order.FillHeader(headerId, robotTopicPrefix, timeStamp);
        order.OrderId = orderId;
        order.OrderUpdateId = 0;
        order.ZoneSetId = robotOrderRequest.Request.ZoneSetId;
        order.Nodes = robotOrderRequest.Request.Nodes
            .Select((x, index) => NodeMessage.CreateNodeMessage(x, (uint)index))
            .ToList();
        order.Edges = robotOrderRequest.Request.Edges
            .Select((x, index) => EdgeMessage.CreateEdgeMessage(x, (uint)index))
            .ToList();
        
        
        return order;
    }
    
    public static OrderMessage CreateOrderUpdateMessage(
        uint headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        uint orderUpdateId,
        RobotOrderUpdateRequest robotOrderUpdateRequest)
    {
        var order = new OrderMessage();
        order.FillHeader(headerId, robotTopicPrefix, timeStamp);
        order.OrderId = robotOrderUpdateRequest.Request.OrderId!.Value;
        order.OrderUpdateId = orderUpdateId;
        order.ZoneSetId = robotOrderUpdateRequest.Request.ZoneSetId;
        order.Nodes = robotOrderUpdateRequest.Request.Nodes
            .Select((x, index) => NodeMessage.CreateNodeMessage(x, (uint)index))
            .ToList();
        order.Edges = robotOrderUpdateRequest.Request.Edges
            .Select((x, index) => EdgeMessage.CreateEdgeMessage(x, (uint)index))
            .ToList();
        
        
        return order;
    }
}
