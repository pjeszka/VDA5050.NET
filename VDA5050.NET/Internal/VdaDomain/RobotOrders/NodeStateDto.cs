using VDA5050.NET.Public.Messages.State;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record NodeStateDto(string Id, uint SequenceId, bool Released)
{
    public static NodeStateDto FromMessage(NodeState nodeState)
    {
        return new NodeStateDto(nodeState.NodeId, nodeState.SequenceId, nodeState.Released);
    }
}