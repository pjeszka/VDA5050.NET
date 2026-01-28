using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed record OrderRequestState
{
    public OrderRequestState(
        OrderId orderId,
        RobotOrderRequest orderRequest)
    {
        RobotSerialNumber = orderRequest.RobotSerialNumber;
        Id = new OrderRequestId(orderId, new OrderUpdateId(0));
        Nodes = orderRequest.Nodes;
        Edges = orderRequest.Edges;
        ZoneSetId = orderRequest.ZoneSetId;
        Status = OrderRequestStatus.Requested;
        Message = null;       
    }
    
    public OrderRequestState(
        OrderUpdateId orderUpdateId,
        RobotOrderUpdateRequest orderRequest)
    {
        RobotSerialNumber = orderRequest.RobotSerialNumber;
        Id = new OrderRequestId(orderRequest.OrderId, orderUpdateId);
        Nodes = orderRequest.Nodes;
        Edges = orderRequest.Edges;
        ZoneSetId = orderRequest.ZoneSetId;
        Status = OrderRequestStatus.Requested;
        Message = null;       
    }
    
    

    public void Update(OrderRequestStatus status, string? message = null, DateTime? sentAt = null)
    {
        Status = status;
        Message = message;
        SentAt = sentAt;
    }
    
    public bool HasEnded =>
        Status is OrderRequestStatus.Invalid
            or OrderRequestStatus.Rejected
            or OrderRequestStatus.Accepted;

    public RobotSerialNumber RobotSerialNumber { get; private set; }
    public OrderRequestId Id { get; private set; }
    public List<Node> Nodes { get; set; }
    public List<Edge> Edges { get; set; }
    public string? ZoneSetId { get; set; }
    public OrderRequestStatus Status { get; private set; }
    public DateTime? SentAt { get; private set; }
    public string? Message { get;  private set; } = null;
}