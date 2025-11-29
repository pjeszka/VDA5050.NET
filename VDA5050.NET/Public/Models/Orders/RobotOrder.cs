using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record RobotOrder(RobotSerialNumber RobotSerialNumber, NewOrderDto Order);

public sealed record NewOrderDto(List<Node> Nodes, List<Edge> Edges, string? ZoneSetId = null);

public sealed record RobotOrderUpdate(RobotSerialNumber RobotSerialNumber, OrderUpdateDto OrderUpdate);
public sealed record OrderUpdateDto(OrderId orderId, List<Node> Nodes, List<Edge> Edges, string? ZoneSetId = null);