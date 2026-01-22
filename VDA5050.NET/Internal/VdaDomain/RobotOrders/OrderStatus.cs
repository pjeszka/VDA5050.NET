namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public enum OrderStatus
{
    Unknown, // -> Temporary
    // In progress
    Pending,
    Canceling,
    // Has ended
    Failed,
    Canceled,
    Finished 
}