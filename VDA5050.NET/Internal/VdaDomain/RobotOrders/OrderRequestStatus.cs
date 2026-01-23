namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public enum OrderRequestStatus
{
    // In progress
    Requested,
    Sent,
    // Has ended
    Invalid,
    Rejected,
    Accepted
}