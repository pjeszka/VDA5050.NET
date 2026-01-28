namespace VDA5050.NET.Public.Models.Orders.OrderRequesting;

public sealed record RobotOrderRequest
{
    public RobotOrderRequest(
        RobotSerialNumber robotSerialNumber,
        List<Node> nodes,
        List<Edge> edges,
        string? zoneSetId = null)
    {
        RobotSerialNumber = robotSerialNumber;
        Nodes = nodes;
        Edges = edges;
        ZoneSetId = zoneSetId;
    }

    public RobotSerialNumber RobotSerialNumber { get; set; }
    public List<Node> Nodes { get; set; }
    public List<Edge> Edges { get; set; }
    public string? ZoneSetId { get; set; }
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
        OrderId = orderId;
        Nodes = nodes;
        Edges = edges;
        ZoneSetId = zoneSetId;
    }
    public RobotSerialNumber RobotSerialNumber { get; set; }
    public OrderId OrderId { get; set; }
    public List<Node> Nodes { get; set; }
    public List<Edge> Edges { get; set; }
    public string? ZoneSetId { get; set; }
    
    
}
