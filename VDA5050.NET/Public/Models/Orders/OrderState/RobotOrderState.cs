namespace VDA5050.NET.Public.Models.Orders.OrderState;

public record RobotOrderState(
    RobotSerialNumber RobotSerialNumber,
    OrderId OrderId,
    OrderUpdateId OrderUpdateId,
    string LastNodeId,
    uint LastNodeSequenceId,
    ICollection<EdgeState> EdgeState,
    ICollection<NodeState> NodeState,
    ICollection<ActionState> ActionStates
    RobotOrderStatus );