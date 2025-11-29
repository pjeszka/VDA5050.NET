namespace VDA5050.NET.Public.Models.Orders;

public sealed record RobotOrder(RobotSerialNumber RobotSerialNumber, NewOrderDto Order);

public sealed record NewOrderDto(List<NodeDto> Nodes, List<EdgeDto> Edges, string? ZoneSetId = null);

public sealed record RobotOrderUpdate(RobotSerialNumber RobotSerialNumber, OrderUpdateDto OrderUpdate);
public sealed record OrderUpdateDto(OrderId OrderId, List<NodeDto> Nodes, List<EdgeDto> Edges, string? ZoneSetId = null);