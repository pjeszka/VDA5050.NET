using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record NodeState(string Id, uint SequenceId, bool Released, NodeP)
{
    internal static NodeState FromMessage(NodeStateMessage nodeStateMessage)
    {
        return new NodeState(nodeStateMessage.NodeId, nodeStateMessage.SequenceId, nodeStateMessage.Released);
    }
}