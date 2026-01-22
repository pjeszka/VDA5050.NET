using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record OrderDetails(
    bool IsFinished,
    string? LastNodeId,
    uint? LastNodeSequenceId,
    List<NodeState> NodeStates,
    List<EdgeState> EdgeStates,
    List<ActionState> ActionStates)
{
    internal static OrderDetails FromRobotStateMessage(StateMessage robotStateMessage)
    {
        // TODO decide how to know if order is finished
        var lastNodeState = robotStateMessage.NodeStates.Last();
        var isFinished = lastNodeState.NodeId == robotStateMessage.LastNodeId && lastNodeState.SequenceId == robotStateMessage.LastNodeSequenceId;
        return new OrderDetails(
            isFinished,
            robotStateMessage.LastNodeId,
            robotStateMessage.LastNodeSequenceId,
            robotStateMessage.NodeStates.Select(NodeState.FromMessage).ToList(),
            robotStateMessage.EdgeStates.Select(EdgeState.FromMessage).ToList(),
            robotStateMessage.ActionStates.Select(ActionState.FromMessage).ToList());
    }
}