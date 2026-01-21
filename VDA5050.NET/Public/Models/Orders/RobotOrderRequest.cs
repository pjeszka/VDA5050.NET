namespace VDA5050.NET.Public.Models.Orders;

public sealed record RobotOrderRequest
{
    public RobotOrderRequest(RobotSerialNumber robotSerialNumber,
        List<NodeDto> nodes,
        List<EdgeDto> edges,
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
        List<NodeDto> nodes,
        List<EdgeDto> edges,
        string? zoneSetId = null)
    {
        RobotSerialNumber = robotSerialNumber;
        Request = new OrderRequest(orderId, nodes, edges, zoneSetId);
    }
    public RobotSerialNumber RobotSerialNumber { get; }
    public OrderRequest Request { get; }
}

public record OrderRequest(OrderId? OrderId, List<NodeDto> Nodes, List<EdgeDto> Edges, string? ZoneSetId = null);


