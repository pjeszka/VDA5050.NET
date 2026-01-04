using VDA5050.NET.Public.Messages.State;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record OrderState(
    bool IsFinished,
    string? LastNodeId,
    uint? LastNodeSequenceId,
    List<NodeStateDto> NodeStates,
    List<EdgeStateDto> EdgeStates,
    List<ActionStateDto> ActionStates)
{
    public static OrderState FromRobotState(State robotState)
    {
        // TODO decide how to know if order is finished
        var lastNodeState = robotState.NodeStates.Last();
        var isFinished = lastNodeState.NodeId == robotState.LastNodeId && lastNodeState.SequenceId == robotState.LastNodeSequenceId;
        return new OrderState(
            isFinished,
            robotState.LastNodeId,
            robotState.LastNodeSequenceId,
            robotState.NodeStates.Select(NodeStateDto.FromMessage).ToList(),
            robotState.EdgeStates.Select(EdgeStateDto.FromMessage).ToList(),
            robotState.ActionStates.Select(ActionStateDto.FromMessage).ToList());
    }

    public static OrderState FromRobotOrderRequest(RobotOrder robotOrder)
    {
        
    }
}