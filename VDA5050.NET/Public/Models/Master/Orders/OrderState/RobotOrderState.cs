using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Public.Enums.Domain;

namespace VDA5050.NET.Public.Models.Orders.OrderState;

public record RobotOrderState(
    OrderId? OrderId,
    OrderUpdateId? OrderUpdateId,
    string LastNodeId,
    uint LastNodeSequenceId,
    ICollection<EdgeState> EdgeStates,
    ICollection<NodeState> NodeStates,
    ICollection<ActionState> ActionStates,
    OrderStatus Status)
{
    internal static RobotOrderState FromRobotStateMessage(StateMessage robotStateMessage, OrderStatus robotOrderStatus)
    {
        return new RobotOrderState(
            robotStateMessage.OrderId is null ? null : new OrderId(robotStateMessage.OrderId),
            robotStateMessage.OrderUpdateId is null ? null : new OrderUpdateId(robotStateMessage.OrderUpdateId.Value),
            robotStateMessage.LastNodeId,
            robotStateMessage.LastNodeSequenceId,
            robotStateMessage.EdgeStates.Select(EdgeState.FromMessage).ToList(),
            robotStateMessage.NodeStates.Select(NodeState.FromMessage).ToList(),
            robotStateMessage.ActionStates.Select(ActionState.FromMessage).ToList(),
            robotOrderStatus);
    }
}