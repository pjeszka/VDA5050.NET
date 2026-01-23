using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed record OrderRequestState
{
    public OrderRequestState(
        OrderId orderId,
        RobotOrderRequest orderRequest)
    {
        RobotSerialNumber = orderRequest.RobotSerialNumber;
        Id = new OrderRequestId(orderId, new OrderUpdateId(0));
        OrderRequest = orderRequest.Request;
        Status = OrderRequestStatus.Requested;
        Message = null;       
    }
    
    public OrderRequestState(
        OrderUpdateId orderUpdateId,
        RobotOrderUpdateRequest orderRequest)
    {
        RobotSerialNumber = orderRequest.RobotSerialNumber;
        Id = new OrderRequestId(orderRequest.Request.OrderId!, orderUpdateId);
        OrderRequest = orderRequest.Request;
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
    public OrderRequest OrderRequest { get; private set; }
    public OrderRequestStatus Status { get; private set; }
    public DateTime? SentAt { get; private set; }
    public string? Message { get;  private set; } = null;
}