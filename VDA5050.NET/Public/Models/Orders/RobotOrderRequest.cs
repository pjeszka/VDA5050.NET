namespace VDA5050.NET.Public.Models.Orders;

public sealed record RobotOrderRequest
{
    public RobotOrderRequest(
        RobotSerialNumber robotSerialNumber,
        List<Node> nodes,
        List<Edge> edges,
        string? zoneSetId = null)
    {
        RobotSerialNumber = robotSerialNumber;
        Request = new OrderRequest(null, nodes, edges, zoneSetId);
    }

    public RobotSerialNumber RobotSerialNumber { get; }
    public OrderRequest Request { get; }
}

public sealed record RobotOrderUpdateRequest
{
    public RobotOrderUpdateRequest(RobotSerialNumber robotSerialNumber,
        OrderId orderId,
        List<Node> nodes,
        List<Edge> edges,
        string? zoneSetId = null)
    {
        RobotSerialNumber = robotSerialNumber;
        Request = new OrderRequest(orderId, nodes, edges, zoneSetId);
    }
    public RobotSerialNumber RobotSerialNumber { get; }
    public OrderRequest Request { get; }
}

public record OrderRequest(OrderId? OrderId, List<Node> Nodes, List<Edge> Edges, string? ZoneSetId = null);


