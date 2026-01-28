using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

namespace VDA5050.NET.Public.Models.Orders.OrderState;

public sealed record NodeState(string Id, uint SequenceId, bool Released, NodePose? NodePose)
{
    internal static NodeState FromMessage(NodeStateMessage nodeStateMessage)
    {
        return new NodeState(
            nodeStateMessage.NodeId,
            nodeStateMessage.SequenceId,
            nodeStateMessage.Released,
            NodePose.FromMessage(nodeStateMessage.NodePosition));
    }
}