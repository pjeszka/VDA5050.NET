namespace VDA5050.NET.Public.Enums.Domain;

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