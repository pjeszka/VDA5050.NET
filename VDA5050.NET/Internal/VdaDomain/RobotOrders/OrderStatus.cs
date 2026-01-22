namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public enum OrderStatus
{
    // In progress
    Pending,
    Canceling,
    // Has ended
    Failed,
    Canceled,
    Finished 
}