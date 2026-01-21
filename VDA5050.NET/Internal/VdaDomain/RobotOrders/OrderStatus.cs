namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public enum OrderStatus
{
    // In progress
    Requested,
    Send,
    Pending,
    Canceling,
    // Has ended
    Rejected,
    Invalid,
    Failed,
    Canceled,
    Finished 
    
    

}