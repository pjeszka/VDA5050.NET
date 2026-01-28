namespace VDA5050.NET.Public.Enums.Domain;

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